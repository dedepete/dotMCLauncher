using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class Argument
    {
        [JsonIgnore]
        public ArgumentType Type { get; protected set; }
    }
}
