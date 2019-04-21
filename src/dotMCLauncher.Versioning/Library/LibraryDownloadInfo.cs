using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class LibraryDownloadInfo
    {
        public Dictionary<string, DownloadEntry> Classifiers { get; set; }

        public DownloadEntry Artifact { get; set; }

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string classifier = null)
        {
            List<DownloadEntry> entries = new List<DownloadEntry>();

            if (Artifact != null) {
                entries.Add(Artifact);
            }

            if (classifier == null || Classifiers == null) {
                return entries;
            }

            if (Classifiers.ContainsKey(classifier)) {
                Classifiers[classifier].IsNatives = true;
                entries.Add(Classifiers[classifier]);
            } else {
                throw new KeyNotFoundException($"A specified classifier is not presented in list: '{classifier}'.");
            }

            return entries;
        }
    }
}
