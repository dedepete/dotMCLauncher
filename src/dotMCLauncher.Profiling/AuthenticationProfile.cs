using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class AuthenticationProfile : JsonSerializable
    {
        public string DisplayName { get; set; }
    }
}
