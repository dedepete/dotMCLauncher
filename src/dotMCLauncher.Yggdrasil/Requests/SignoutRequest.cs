using Newtonsoft.Json;

namespace dotMCLauncher.Yggdrasil
{
    public class SignoutRequest : BaseRequest
    {
        public SignoutRequest(string username, string password)
        {
            Username = username;
            Password = password;

            Url = UrlProvider.SignoutUrl;
        }

        #region Request

        [JsonProperty("username")]
        private string Username { get; set; }

        [JsonProperty("password")]
        private string Password { get; set; }

        #endregion

        public override bool? WasSuccessful
            => StatusCode == 204;

        protected override BaseRequest Parse(string json)
        {
            return this;
        }
    }
}
