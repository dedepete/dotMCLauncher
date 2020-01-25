using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Resourcing
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class Asset
    {
        [JsonIgnore]
        public string AssociatedName { get; set; }

        public string Hash { get; set; }

        public int Size { get; set; }

        public string GetDirectoryName() => Hash?.Substring(0, 2);

        public string GetPath() => GetDirectoryName() + @"\" + Hash;
    }
}
