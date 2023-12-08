using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class OsConditions
    {
        /// <summary>
        /// Operating system's name. Valid values are `windows`, 'linux', 'os' (not `macos`)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Operating system's version. Can be a regular expression pattern, but must be a target string when using in `CheckIfMeetsConditions(OsConditions)`.
        /// </summary>
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
                    if (osConditions.Version != null && !Regex.Match(osConditions.Version, Version, RegexOptions.IgnoreCase).Success) {
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
