using System.Net;

namespace dotMCLauncher.Yggdrasil
{
    public static class RequestLogger
    {
        public delegate void RequestSentHandler(HttpWebRequest request, string json);

        public delegate void ResponseReceivedHandler(WebResponse response, string json);

        public static event RequestSentHandler RequestSent;
        public static event ResponseReceivedHandler ResponseReceived;

        internal static void Sent(HttpWebRequest request, string json)
            => RequestSent?.Invoke(request, json);

        internal static void Received(WebResponse response, string json)
            => ResponseReceived?.Invoke(response, json);
    }
}
