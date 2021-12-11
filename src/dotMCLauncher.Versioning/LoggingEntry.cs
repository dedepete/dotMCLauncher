using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LoggingEntry
    {
        public string Argument { get; set; }
        public DownloadEntry File { get; set; }
        public string Type { get; set; }
    }
}
