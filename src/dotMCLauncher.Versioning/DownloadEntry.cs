using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class DownloadEntry
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string Sha1 { get; set; }
        public int Size { get; set; }

        [JsonIgnore]
        public bool IsNatives { get; set; }
    }
}
