using System.Collections.Generic;
using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class Library
    {
        /// <summary>
        /// Library's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Library's download info.
        /// </summary>
        [JsonProperty("downloads")]
        public LibraryDownloadInfo DownloadInfo { get; set; }

        /// <summary>
        /// Library's download URL. Not being used in official versions, but may be used by modded versions.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("natives")]
        public Dictionary<string, string> Natives { get; set; }

        [JsonProperty("rules")]
        private RuleCollection _rules { get; set; }

        public bool IsAllowed(RuleConditions conditions) => _rules?.CheckIfAllowed(conditions) ?? true;

        /// <summary>
        /// Returns true, if contains any natives.
        /// </summary>
        public bool IsNatives => Natives != null;

        /// <summary>
        /// Returns true, if contains natives for specified operating system.
        /// </summary>
        public bool IsNativesFor(string operatingSystem) => IsNatives && Natives.ContainsKey(operatingSystem);

        public string GetPath(string operatingSystem = "windows")
        {
            return GetPath(operatingSystem, false);
        }

        public string GetPath(string operatingSystem, bool is64BitArchitecture)
        {
            string[] s = Name.Split(':');
            return string.Format(@"{0}\{1}\{2}\{1}-{2}" +
                                 (IsNativesFor(operatingSystem)
                                     ? "-" + Natives[operatingSystem].Replace("${arch}", is64BitArchitecture ? "64" : "32")
                                     : string.Empty) + ".jar",
                s[0].Replace('.', '\\'), s[1], s[2]);
        }

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string operatingSystem = "windows")
        {
            return GetDownloadsEntries(operatingSystem, false);
        }

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string operatingSystem, bool is64BitArchitecture)
        {
            return GetDownloadsEntries(@"https://libraries.minecraft.net/", operatingSystem, is64BitArchitecture);
        }

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string baseRepository, string operatingSystem, bool is64BitArchitecture)
        {
            List<DownloadEntry> entries = new List<DownloadEntry>();

            if (DownloadInfo != null) {
                foreach (DownloadEntry downloadEntry in DownloadInfo.GetDownloadsEntries(IsNatives && operatingSystem != null && Natives.ContainsKey(operatingSystem)
                    ? Natives[operatingSystem].Replace("${arch}", is64BitArchitecture ? "64" : "32")
                    : null)) {
                    entries.Add(downloadEntry);
                }
            } else {
                entries.Add(new DownloadEntry {
                    Url = Url ?? baseRepository + GetPath(operatingSystem), Path = GetPath(), IsNatives = IsNatives
                });
            }

            return entries;
        }
    }
}
