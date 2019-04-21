using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public abstract class BaseVersion
    {
        /// <summary>
        /// Version ID.
        /// </summary>
        [JsonProperty("id")]
        public string VersionId { get; set; }

        /// <summary>
        /// Build type.
        /// </summary>
        [JsonProperty("type")]
        public string ReleaseType { get; set; }
    }
}
