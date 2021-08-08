using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Versioning
{
    public class Argument
    {
        [JsonIgnore]
        public ArgumentType Type
        {
            get; protected set;
        }

        [JsonIgnore]
        public virtual JToken Value
        {
            get; set;
        }
    }
}
