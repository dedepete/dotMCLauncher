using Newtonsoft.Json;

namespace dotMCLauncher.Yggdrasil
{
    public class InvalidateRequest : BaseRequest
    {
        public InvalidateRequest(string accessToken, string clientToken)
        {
            AccessToken = accessToken;
            ClientToken = clientToken;

            Url = UrlProvider.InvalidateUrl;
        }

        #region Request

        [JsonProperty("accessToken")]
        private string AccessToken { get; set; }

        [JsonProperty("clientToken")]
        private string ClientToken { get; set; }

        #endregion

        public override bool? WasSuccessful
            => StatusCode == 204;

        protected override BaseRequest Parse(string json)
            => this;
    }
}
