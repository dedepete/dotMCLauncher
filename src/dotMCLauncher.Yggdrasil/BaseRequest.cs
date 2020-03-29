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

        public virtual BaseRequest SendRequest()
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

            RequestLogger.Sent(request, Content);

            string json;
            try {
                WebResponse webResponse = request.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader reader =
                    new StreamReader(dataStream ?? throw new EndOfStreamException("Response stream is null."));
                json = reader.ReadToEnd();
                RequestLogger.Received(webResponse, json);
                StatusCode = (int) ((HttpWebResponse) webResponse).StatusCode;
            } catch (WebException ex) {
                WebResponse webResponse = ex.Response;
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader reader =
                    new StreamReader(dataStream ?? throw new EndOfStreamException("Response stream is null."));
                json = reader.ReadToEnd();
                RequestLogger.Received(webResponse, json);
                StatusCode = (int) ((HttpWebResponse) webResponse).StatusCode;
            }

            if (!string.IsNullOrWhiteSpace(json)) {
                Response = JObject.Parse(json);
            }

            return Parse(json);
        }

        protected virtual BaseRequest Parse(string json)
        {
            if (!(JsonConvert.DeserializeObject(json, GetType()) is BaseRequest request)) {
                return null;
            }

            request.Response = Response;
            request.StatusCode = StatusCode;
            return request;

        }

        [JsonIgnore]
        public virtual bool? WasSuccessful
        {
            get {
                if (Response == null) {
                    return null;
                }

                return StatusCode == 200;
            }
        }
    }
}
