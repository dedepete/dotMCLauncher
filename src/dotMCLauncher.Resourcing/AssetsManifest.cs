using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Resourcing
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class AssetsManifest
    {
        [JsonProperty("virtual", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsVirtual { get; set; }

        public Dictionary<string, Asset> Objects { get; set; }

        private void AssociateNames()
        {
            foreach (KeyValuePair<string, Asset> pair in Objects) {
                pair.Value.AssociatedName = pair.Key;
            }
        }

        public static AssetsManifest Parse(string pathToFile)
            => JsonConvert.DeserializeObject(File.ReadAllText(pathToFile), typeof(AssetsManifest)) as AssetsManifest;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
            => AssociateNames();
    }
}
