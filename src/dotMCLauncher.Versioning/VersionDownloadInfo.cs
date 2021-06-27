using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class VersionDownloadInfo
    {
        [JsonProperty("client")]
        public DownloadEntry Client { get; set; }

        [JsonProperty("client_mappings")]
        public DownloadEntry ClientMappings { get; set; }

        [JsonProperty("server")]
        public DownloadEntry Server { get; set; }

        [JsonProperty("server_mappings")]
        public DownloadEntry ServerMappings { get; set; }

        [JsonIgnore]
        public bool IsServerAvailable => Server != null;
    }
}
