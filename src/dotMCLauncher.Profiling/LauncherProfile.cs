using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LauncherProfile : JsonSerializable
    {
        internal string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty]
        private string _icon { get; set; }

        [JsonIgnore]
        public LauncherProfileIcon.Icon Icon
        {
            get => LauncherProfileIcon.GetIcon(_icon);
            set => _icon = LauncherProfileIcon.GetString(value);
        }

        [JsonProperty]
        private string _type { get; set; }

        [JsonIgnore]
        public LauncherProfileType Type
        {
            get {
                switch (_type) {
                    case "latest-release":
                        return LauncherProfileType.LATEST_RELEASE;
                    case "latest-snapshot":
                        return LauncherProfileType.LATEST_SNAPSHOT;
                    default:
                        return LauncherProfileType.CUSTOM;
                }
            }
            set {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (value) {
                    case LauncherProfileType.LATEST_RELEASE:
                        _type = "latest-release";
                        break;
                    case LauncherProfileType.LATEST_SNAPSHOT:
                        _type = "latest-snapshot";
                        break;
                    default:
                        _type = "custom";
                        break;
                }
            }
        }

        public string LastVersionId { get; set; }

        public LauncherProfileResolution Resolution { get; set; }

        public DateTime Created { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);

        public DateTime LastUsed { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);

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
    }
}
