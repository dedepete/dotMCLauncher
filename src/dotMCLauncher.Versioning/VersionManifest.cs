using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class VersionManifest
    {
        [JsonIgnore]
        private string _arguments { get; set; }
        
        public string MinecraftArguments
        {
            get {
                return _arguments;
            }
            set {
                _arguments = value;
                ArgCollection = new ArgumentCollection();
                ArgCollection.Parse(value);
            }
        }
        
        [JsonIgnore]
        public ArgumentCollection ArgCollection { get; set; }

        [JsonProperty("arguments")]
        private JObject ArgumentGroups
        {
            get {
                return ArgGroups != null ? JObject.Parse(JsonConvert.SerializeObject(ArgGroups)) : null;
            }
            set {
                Type = VersionManifestType.V2;
                ArgGroups = new List<ArgumentsGroup>();
                foreach (KeyValuePair<string, JToken> pair in value)
                {
                    ArgumentsGroup group = new ArgumentsGroup();
                    group.Type = pair.Key.ToUpperInvariant() == "GAME"
                        ? ArgumentsGroupType.GAME
                        : ArgumentsGroupType.JVM;
                    group.Arguments = new List<Argument>();
                    JArray array = (JArray)pair.Value;
                    foreach (JToken token in array)
                    {
                        if (token is JValue)
                        {
                            group.Arguments.Add(new SingleArgument
                            {
                                Value = token
                            });
                        }
                        else
                        {
                            ExtendedArgument arg = (ExtendedArgument)
                                JsonConvert.DeserializeObject(token.ToString(), typeof(ExtendedArgument));
                            group.Arguments.Add(arg);
                        }
                    }
                    ArgGroups.Add(group);
                }
            }
        }
    }
}
