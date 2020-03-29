using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LauncherVersion : JsonSerializable
    {
        public int Format { get; set; } = 21;
        public string Name { get; set; } = "2.1.5964";

        public int ProfilesFormat { get; set; } = 2;
    }
}
