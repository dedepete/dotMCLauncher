using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Versioning
{
    public class ExtendedArgument : Argument
    {
        public ExtendedArgument()
        {
            Type = ArgumentType.EXTENDED;
        }

        public bool IsAllowed(RuleConditions conditions) => _rules?.CheckIfAllowed(conditions) ?? true;

        [JsonProperty("value")]
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
                if (array.Count > 1) {
                    HasMultipleArguments = true;
                    foreach (JToken jToken in array) {
                        Values.Add(jToken.ToString());
                    }
                    return;

                }
                Values.Add(array[0].ToString());
            }
        }

        [JsonProperty("rules")]
        private RuleCollection _rules { get; set; }

        public List<string> Values { get; set; }

        public bool HasMultipleArguments { get; set; }
    }
}
