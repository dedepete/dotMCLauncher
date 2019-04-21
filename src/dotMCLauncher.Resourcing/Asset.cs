using Newtonsoft.Json;

namespace dotMCLauncher.Resourcing
{
    public class Asset
    {
        [JsonIgnore]
        public string AssociatedName { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        public string GetDirectoryName() => Hash?.Substring(0, 2);

        public string GetPath() => GetDirectoryName() + @"\" + Hash;
    }
}
