using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Yggdrasil
{
    public class RefreshRequest : BaseRequest, IAuthenticateResponse
    {
        public RefreshRequest(string accessToken, string clientToken)
        {
            AccessToken = accessToken;
            ClientToken = clientToken;

            Url = UrlProvider.RefreshUrl;
        }

        #region Request

        [JsonProperty("requestUser")]
        public bool RequestUser { get; set; } = true;

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
