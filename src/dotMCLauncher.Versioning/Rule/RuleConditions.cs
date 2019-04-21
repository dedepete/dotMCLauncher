using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class RuleConditions
    {
        [JsonProperty("os")]
        public OsConditions OsConditions { get; set; }

        [JsonProperty("features")]
        public FeatureConditions FeatureConditions { get; set; }
    }
}
