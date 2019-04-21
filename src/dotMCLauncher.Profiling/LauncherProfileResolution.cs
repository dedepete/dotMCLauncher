using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LauncherProfileResolution : JsonSerializable
    {
        public int Width { get; set; } = 854;

        public int Height { get; set; } = 481;
    }
}
