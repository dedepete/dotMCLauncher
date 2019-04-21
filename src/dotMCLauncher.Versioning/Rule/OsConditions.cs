using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class OsConditions
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("arch")]
        public string Architecture { get; set; }

        public bool CheckIfMeetsConditions(OsConditions osConditions)
        {
            if (Name != null) {
                if (osConditions.Name != null && osConditions.Name != Name) {
                    return false;
                }

                if (Version != null) {
                    if (osConditions.Version == null && Version != null) {
                        return false;
                    }
                }

                if (Architecture != null) {
                    if (osConditions.Architecture != null && osConditions.Architecture != Architecture) {
                        return false;
                    }
                }
            }

            if (Architecture == null) return true;
            return osConditions.Architecture == Architecture;
        }
    }
}
