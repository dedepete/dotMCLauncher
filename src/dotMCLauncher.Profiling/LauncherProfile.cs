using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptIn)]
    public class LauncherProfile : JsonSerializable
    {
        [JsonIgnore]
        public string Id { get; internal set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty("icon")]
        private string _icon
        {
            get => LauncherProfileIcon.GetString(Icon);
            set => Icon = LauncherProfileIcon.GetIcon(value);
        }

        [JsonIgnore]
        public LauncherProfileIcon.Icon Icon { get; set; } = LauncherProfileIcon.Icon.BEDROCK;

        [JsonProperty("type")]
        private string _type
        {
            get {
                switch (Type) {
                    case LauncherProfileType.LATEST_RELEASE:
                        return "latest-release";
                    case LauncherProfileType.LATEST_SNAPSHOT:
                        return "latest-snapshot";
                    default:
                        return "custom";
                }
            }
            set {
                switch (value) {
                    case "latest-release":
                        Type = LauncherProfileType.LATEST_RELEASE;
                        break;
                    case "latest-snapshot":
                        Type = LauncherProfileType.LATEST_SNAPSHOT;
                        break;
                    default:
                        Type = LauncherProfileType.CUSTOM;
                        break;
                }
            }
        }

        [JsonIgnore]
        public LauncherProfileType Type { get; set; } = LauncherProfileType.LATEST_RELEASE;

        [JsonProperty]
        public string LastVersionId { get; set; }

        [JsonProperty]
        public LauncherProfileResolution Resolution { get; set; }

        [JsonProperty]
        public DateTime Created { get; set; } = DateTime.Now;

        [JsonProperty]
        public DateTime LastUsed { get; set; } = DateTime.Now;

        [JsonProperty("gameDir")]
        public string GameDirectory { get; set; }

        [JsonProperty("javaDir")]
        public string JavaDirectory { get; set; }

        [JsonProperty("javaArgs")]
        public string JavaArguments { get; set; }

        [JsonProperty("logConfig")]
        public string LogConfiguration { get; set; }

        [JsonProperty("logConfigIsXML", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLogConfigurationXml { get; set; }

        public override string ToString()
            => $"{{\"{Name}\",{Type},{Id}}}";
    }
}
