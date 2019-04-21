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

        public override BaseRequest MakeRequest()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.Method = "GET";

            string response;
            try {
                Stream dataStream = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                response = reader.ReadToEnd();
                StatusCode = (int) (request.GetResponse() as HttpWebResponse).StatusCode;
            }
            catch (WebException ex) {
                Stream dataStream = ex.Response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                response = reader.ReadToEnd();
                StatusCode = (int) (ex.Response as HttpWebResponse).StatusCode;
            }

            if (!string.IsNullOrWhiteSpace(response)) {
                Response = JObject.Parse(response);
            }

            return Parse(response);
        }

        protected override BaseRequest Parse(string json)
        {
            BaseRequest request = JsonConvert.DeserializeObject(json, GetType()) as BaseRequest;
            request.Response = Response;
            request.StatusCode = StatusCode;
            return request;
        }
    }
}
