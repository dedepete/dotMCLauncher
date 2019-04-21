using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class AuthenticationEntry : JsonSerializable
    {
        public string AccessToken { get; set; }

        public string Username { get; set; }

        [JsonProperty("profiles")]
        public Dictionary<string, AuthenticationProfile> AuthenticationProfiles { get; set; }
    }
}
