using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class OsConditions
    {
        public string Name { get; set; }

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

            if (Architecture == null) {
                return true;
            }

            return osConditions.Architecture == Architecture;
        }
    }
}
