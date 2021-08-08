using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class SingleArgument : Argument
    {
        public SingleArgument(JToken value)
        {
            Type = ArgumentType.SINGLE;
            Value = value;
        }

        public override string ToString()
            => Value.ToString();
    }
}
