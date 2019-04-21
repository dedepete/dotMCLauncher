using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class RawVersionListManifestEntry : BaseVersion
    {
        [JsonProperty("url")]
        public string ManifestUrl { get; set; }
    }
}
