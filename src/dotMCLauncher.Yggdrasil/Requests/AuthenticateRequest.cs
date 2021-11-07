using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Yggdrasil
{
    public class AuthenticateRequest : BaseRequest, IAuthenticateResponse
    {
        public AuthenticateRequest(string email, string password, string clientToken = null)
        {
            Username = email;
            Password = password;
            ClientToken = clientToken;

            Url = UrlProvider.AuthenticateUrl;
        }

        #region Request

        [JsonProperty("agent")]
        private JObject Agent { get; set; } = new JObject {
            {
                "name", "Minecraft"
            }, {
                "version", 1
            }
        };

        [JsonProperty("username")]
        public string Username { get; protected set; }

        [JsonProperty("password")]
        private string Password { get; set; }

        [JsonProperty("requestUser")]
        public bool RequestUser { get; protected set; } = true;

        #endregion

        #region IAuthenticateResponse

        public string AccessToken { get; set; }
        public string ClientToken { get; set; }
        public JObject SelectedProfile { get; set; }
        public JObject User { get; set; }
        public string Uuid => SelectedProfile["id"].ToString();

        #endregion
    }
}
