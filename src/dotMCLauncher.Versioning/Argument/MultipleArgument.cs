using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class MultipleArgument : Argument
    {
        public MultipleArgument()
        {
            Type = ArgumentType.MULTIPLE;
        }

        public bool IsAllowed(RuleConditions conditions) => _rules?.CheckIfAllowed(conditions) ?? true;

        public JToken Value
        {
            get {
                if (!HasMultipleArguments) {
                    return new JValue(Values?[0]);
                }

                JArray array = new JArray();
                foreach (string value in Values) {
                    array.Add(value);
                }

                return array;
            }
            set {
                Values = new List<string>();
                JArray array = value.Type == JTokenType.Array ? value as JArray : new JArray(value);
                if (array != null && array.Count > 1) {
                    HasMultipleArguments = true;
                    foreach (JToken jToken in array) {
                        Values.Add(jToken.ToString());
                    }

                    return;
                }

                Values.Add(array?[0].ToString());
            }
        }

        [JsonProperty("rules")]
        private RuleCollection _rules { get; set; }

        [JsonIgnore]
        public List<string> Values { get; set; }

        [JsonIgnore]
        public bool HasMultipleArguments { get; set; }
    }
}
