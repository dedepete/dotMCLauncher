using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using dotMCLauncher.Yggdrasil;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace dotMCLauncher.Tests.Yggdrasil
{
    [Explicit("Some of the end-points in these tests are severely rate-limited.")]
    public class AuthenticationTests
    {
        [SetUp]
        public void SetUp()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets("d16634c5-938e-4a63-b980-076e03708d72")
                .Build();

            RequestLogger.RequestSent += delegate(HttpWebRequest request, string json) {
                                             TestContext.WriteLine((
                                                 $"=> {request.Method} ${request.Address} {(string.IsNullOrWhiteSpace(json) ? string.Empty : $"\n{DepersonalizeLog(JObject.Parse(json))}")}"
                                             ));
                                         };

            RequestLogger.ResponseReceived += delegate(WebResponse response, string json) {
                                                  TestContext.WriteLine((
                                                      $"=< {((HttpWebResponse) response).StatusCode} ${response.ResponseUri} {(string.IsNullOrWhiteSpace(json) ? string.Empty : $"\n{DepersonalizeLog(JObject.Parse(json))}")}"
                                                  ));
                                              };
        }

        private IConfigurationRoot Configuration { get; set; }

        private string _accessToken = string.Empty;
        private string _clientToken = string.Empty;

        [Test, Order(1)]
        public void ValidateInvalidTokens()
            => Assert.False(Validate("foo", "bar"));

        [Test, Order(2)]
        public void AuthenticateAndValidate()
        {
            if (string.IsNullOrWhiteSpace(Configuration["Yggdrasil:username"]) ||
                string.IsNullOrWhiteSpace(Configuration["Yggdrasil:password"])) {
                Assert.Ignore(
                    "Secrets have not been configured.\nPlease, provide `Yggdrasil:username` and `Yggdrasil:password` to perform this test.\nSee https://docs.microsoft.com/en-gb/aspnet/core/security/app-secrets#set-a-secret");
            }

            AuthenticateRequest authenticateRequest = null;

            Authenticate(Configuration["Yggdrasil:username"], Configuration["Yggdrasil:password"],
                ref authenticateRequest);

            Assert.True(Validate(authenticateRequest.AccessToken, authenticateRequest.ClientToken),
                $"Error: {authenticateRequest.Error}\nMessage: {authenticateRequest.ErrorMessage}\nCaused by: {authenticateRequest.Cause}");
            _accessToken = authenticateRequest.AccessToken;
            _clientToken = authenticateRequest.ClientToken;
        }

        [Test]
        public void AuthenticateWithInvalidCredentials()
        {
            AuthenticateRequest authenticateRequest = null;

            Authenticate("foo@example.example", "bar", ref authenticateRequest);

            Assert.False(authenticateRequest.WasSuccessful);
        }

        [Test, Order(3)]
        public void RefreshAndValidate()
        {

            if (string.IsNullOrWhiteSpace(_accessToken) && string.IsNullOrWhiteSpace(_clientToken)) {
                AuthenticateAndValidate();
            }

            RefreshRequest refreshRequest = null;

            Refresh(_accessToken, _clientToken, ref refreshRequest);

            Assert.True(Validate(refreshRequest.AccessToken, refreshRequest.ClientToken),
                $"Error: {refreshRequest.Error}\nMessage: {refreshRequest.ErrorMessage}\nCaused by: {refreshRequest.Cause}");

            _accessToken = refreshRequest.AccessToken;
            _clientToken = refreshRequest.ClientToken;
        }

        [Test]
        public void RefreshWithInvalidCredentials()
        {
            RefreshRequest refreshRequest = null;

            Refresh("foo", "bar", ref refreshRequest);

            Assert.False(refreshRequest.WasSuccessful);
        }

        [Test, Order(4)]
        public void InvalidateValidTokens()
        {
            if (string.IsNullOrWhiteSpace(_accessToken) && string.IsNullOrWhiteSpace(_clientToken)) {
                AuthenticateAndValidate();
            }

            Assert.True(Invalidate(_accessToken, _clientToken));
        }

        [Test]
        [Ignore("For some reason returns True even for invalid tokens.")]
        public void InvalidateInvalidTokens()
            => Assert.False(Invalidate("foo", "bar"));

        [Test]
        public void Signout()
        {
            if (string.IsNullOrWhiteSpace(Configuration["Yggdrasil:username"]) ||
                string.IsNullOrWhiteSpace(Configuration["Yggdrasil:password"])) {
                Assert.Ignore(
                    "Secrets have not been configured.\nPlease, provide `Yggdrasil:username` and `Yggdrasil:password` to perform this test.\nSee https://docs.microsoft.com/en-gb/aspnet/core/security/app-secrets#set-a-secret");
            }

            SignoutRequest signoutRequest =
                new SignoutRequest(Configuration["Yggdrasil:username"], Configuration["Yggdrasil:password"]);

            signoutRequest = signoutRequest.SendRequest() as SignoutRequest;

            Assert.True(signoutRequest?.WasSuccessful,
                $"Error: {signoutRequest?.Error}\nMessage: {signoutRequest?.ErrorMessage}\nCaused by: {signoutRequest?.Cause}");
        }

        [Test]
        public void SignoutWithInvalidCredentials()
        {
            SignoutRequest signoutRequest = new SignoutRequest("foo", "bar");

            signoutRequest = signoutRequest.SendRequest() as SignoutRequest;

            Assert.False(signoutRequest?.WasSuccessful);
        }

        [Test]
        public void GetUsername()
        {
            UsernameRequest usernameRequest = new UsernameRequest("689b94ff553b4c179e662e0865b12dcb");

            usernameRequest = usernameRequest.SendRequest() as UsernameRequest;

            Assert.AreEqual("dedepete", usernameRequest?.Username);
        }

        [Test]
        public void GetUsernameByInvalidUuid()
        {
            UsernameRequest usernameRequest = new UsernameRequest("foobar");

            usernameRequest = usernameRequest.SendRequest() as UsernameRequest;

            Assert.False(usernameRequest?.WasSuccessful);
        }

        private static bool Invalidate(string accessToken, string clientToken)
        {
            InvalidateRequest invalidateRequest =
                new InvalidateRequest(accessToken, clientToken);

            invalidateRequest = invalidateRequest.SendRequest() as InvalidateRequest;
            if (invalidateRequest != null && invalidateRequest.WasSuccessful == true) {
                return invalidateRequest.WasSuccessful ?? false;
            }

            return false;
        }

        private static bool Validate(string accessToken, string clientToken)
        {
            ValidateRequest validateRequest =
                new ValidateRequest(accessToken, clientToken);

            validateRequest = validateRequest.SendRequest() as ValidateRequest;
            if (validateRequest != null && validateRequest.WasSuccessful == true) {
                return validateRequest.WasSuccessful ?? false;
            }

            return false;
        }

        // ReSharper disable once RedundantAssignment
        private static void Authenticate(string username, string password, ref AuthenticateRequest request)
        {
            request = new AuthenticateRequest(username, password);

            request = request.SendRequest() as AuthenticateRequest;
        }

        // ReSharper disable once RedundantAssignment
        private static void Refresh(string accessToken, string clientToken, ref RefreshRequest request)
        {
            request = new RefreshRequest(accessToken, clientToken);

            request = request.SendRequest() as RefreshRequest;
        }

        private string DepersonalizeLog(JObject log)
        {
            if (Convert.ToBoolean(Configuration["Yggdrasil:displayCredentials"])) {
                return log.ToString();
            }

            Regex regex = new Regex(@"\""(\w+)\"":\W?\""\S+\""(,)?");

            string toReturn = regex.Replace(log.ToString(),
                match
                    => new[] {"username", "password", "id", "accessToken", "clientToken"}.Contains(
                        match.Groups[1].Value, StringComparer.InvariantCultureIgnoreCase)
                        ? $"\"{match.Groups[1].Value}\": \"<{match.Groups[1].Value}>\"{match.Groups[2].Value}"
                        : match.Value);

            if (Configuration["Yggdrasil:username"] != null) {
                toReturn = toReturn.Replace(Configuration["Yggdrasil:username"],
                    $"<{new string('*', Configuration["Yggdrasil:username"].Length)}>");
            }

            if (Configuration["Yggdrasil:password"] != null) {
                toReturn = toReturn.Replace(Configuration["Yggdrasil:password"],
                    $"<{new string('*', Configuration["Yggdrasil:password"].Length)}>");
            }

            return toReturn;
        }
    }
}
