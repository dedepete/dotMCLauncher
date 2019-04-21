using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LauncherVersion : JsonSerializable
    {
        public string Name { get; set; } = "2.0.1003";

        public int Format { get; set; } = 21;

        public int VersionFormat { get; set; } = 2;
    }
}
