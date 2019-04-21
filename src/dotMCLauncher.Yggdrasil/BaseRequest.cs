using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Yggdrasil
{
    public abstract class BaseRequest : ExceptionResponse
    {
        [JsonIgnore]
        public string Url { get; set; }

        [JsonIgnore]
        public string Content { get; set; }

        [JsonIgnore]
        public JObject Response { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public virtual BaseRequest MakeRequest()
        {
            if (Content == null) {
                Content = JsonConvert.SerializeObject(this, GetType(), Formatting.Indented,
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }

            Response = null;
            Error = null;
            ErrorMessage = null;
            Cause = null;

            byte[] body = Encoding.UTF8.GetBytes(Content);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = body.Length;
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(Content);
                streamWriter.Flush();
                streamWriter.Close();
            }

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

        protected virtual BaseRequest Parse(string json)
        {
            BaseRequest request = JsonConvert.DeserializeObject(json, GetType()) as BaseRequest;
            request.Response = Response;
            request.StatusCode = StatusCode;
            return request;
        }

        public virtual bool? IsSuccessful()
        {
            if (Response == null) {
                return null;
            }

            return StatusCode == 200;
        }
    }
}
