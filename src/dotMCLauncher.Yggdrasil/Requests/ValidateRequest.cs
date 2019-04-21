using Newtonsoft.Json;

namespace dotMCLauncher.Yggdrasil
{
    public class ValidateRequest : BaseRequest
    {
        public ValidateRequest(string accessToken, string clientToken)
        {
            AccessToken = accessToken;
            ClientToken = clientToken;

            Url = UrlProvider.ValidateUrl;
        }

        #region Request

        [JsonProperty("accessToken")]
        private string AccessToken { get; set; }

        [JsonProperty("clientToken")]
        private string ClientToken { get; set; }

        #endregion

        public override bool? IsSuccessful()
        {
            return string.IsNullOrEmpty(Response?.ToString());
        }

        protected override BaseRequest Parse(string json)
        {
            return this;
        }
    }
}
