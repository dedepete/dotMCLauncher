using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Yggdrasil
{
    public class UsernameRequest : BaseRequest
    {
        public UsernameRequest(string uuid)
        {
            Url = "https://sessionserver.mojang.com/session/minecraft/profile/" + uuid;
        }

        #region Response

        [JsonProperty("name")]
        public string Username { get; private set; }

        #endregion

        public override BaseRequest SendRequest()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.Method = "GET";

            string response;
            try {
                WebResponse webResponse = request.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader reader =
                    new StreamReader(dataStream ?? throw new EndOfStreamException("Response stream is null."));
                response = reader.ReadToEnd();
                StatusCode = (int) ((HttpWebResponse) webResponse).StatusCode;
            } catch (WebException ex) {
                Stream dataStream = ex.Response.GetResponseStream();
                StreamReader reader =
                    new StreamReader(dataStream ?? throw new EndOfStreamException("Response stream is null."));
                response = reader.ReadToEnd();
                StatusCode = (int) ((HttpWebResponse) ex.Response).StatusCode;
            }

            if (!string.IsNullOrWhiteSpace(response)) {
                Response = JObject.Parse(response);
            }

            return Parse(response);
        }

        protected override BaseRequest Parse(string json)
            => !(JsonConvert.DeserializeObject(json, GetType()) is BaseRequest) ? this : base.Parse(json);

        public override bool? WasSuccessful
            => !string.IsNullOrEmpty(Username);
    }
}
