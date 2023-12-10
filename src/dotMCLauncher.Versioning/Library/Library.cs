using System;
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
        private string _name { get => Name.ToString();
            set => Name = new LibraryMavenName(value);
        }

        [JsonIgnore]
        public LibraryMavenName Name { get; set; }

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
        public RuleCollection Rules { get; set; }

        public bool IsAllowed(RuleConditions conditions) => Rules?.CheckIfAllowed(conditions) ?? true;

        /// <summary>
        /// Returns true, if contains any natives.
        /// </summary>
        public bool IsNatives => Natives != null || (Name.Classifier?.Contains("natives") ?? false);

        /// <summary>
        /// Returns true, if contains natives for specified operating system.
        /// </summary>
        [Obsolete("The detection of natives has been updated and now requires platform architecture to be specified. This overload will be removed in next release.\nUse `IsNativesFor(LibraryPlatform.OperatingSystem, LibraryPlatform.Architecture)` instead.")]
        public bool IsNativesFor(string operatingSystem)
            => IsNatives && Natives.ContainsKey(operatingSystem);

        /// <summary>
        /// Returns true, if contains natives for specified platform.
        /// </summary>
        public bool IsNativesFor(LibraryPlatform.OperatingSystem operatingSystem, LibraryPlatform.Architecture architecture)
        {
            if (!IsNatives || operatingSystem == LibraryPlatform.OperatingSystem.UNKNOWN) {
                return false;
            }

            if (!(Name.Classifier?.Contains("natives") ?? false)) {
                return Natives.ContainsKey(operatingSystem == LibraryPlatform.OperatingSystem.MACOS
                    ? "osx"
                    : LibraryPlatform.GetString(operatingSystem));
            }

            string[] classifier = Name.Classifier.Split('-');
            if (classifier[1] != LibraryPlatform.GetString(operatingSystem)) {
                return false;
            }

            if (classifier.Length > 2) {
                return classifier[2] == LibraryPlatform.GetString(architecture);
            }

            return true;
        }

        public string GetPath(string operatingSystem = "windows")
            => GetPath(operatingSystem, false);

        public string GetPath(LibraryPlatform.OperatingSystem operatingSystem, LibraryPlatform.Architecture architecture)
            => GetPath(operatingSystem == LibraryPlatform.OperatingSystem.MACOS
                ? "osx"
                : LibraryPlatform.GetString(operatingSystem), architecture != LibraryPlatform.Architecture.X86);

        public string GetPath(string operatingSystem, bool is64BitArchitecture)
        {
            string classifier = Name.Classifier ?? (Natives?.ContainsKey(operatingSystem) ?? false
                ? Natives[operatingSystem]
                : null);

            return string.Format(@"{0}\{1}\{2}\{1}-{2}{3}.{4}",
                Name.GroupId.Replace('.', '\\'), Name.ArtifactId, Name.Version,
                !string.IsNullOrWhiteSpace(classifier)
                    ? "-" + classifier.Replace("${arch}", is64BitArchitecture ? "64" : "32")
                    : string.Empty,
                Name.Type ?? "jar");

            //string[] s = Name.Split(':');
            //return string.Format(@"{0}\{1}\{2}\{1}-{2}" +
            //                     (IsNativesFor(operatingSystem)
            //                         ? "-" + Natives[operatingSystem]
            //                               .Replace("${arch}", is64BitArchitecture ? "64" : "32")
            //                         : string.Empty) + ".jar",
            //    s[0].Replace('.', '\\'), s[1], s[2]);
        }

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string operatingSystem = "windows")
            => GetDownloadsEntries(operatingSystem, false);

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string operatingSystem, bool is64BitArchitecture)
            => GetDownloadsEntries(@"https://libraries.minecraft.net/", operatingSystem, is64BitArchitecture);

        public IEnumerable<DownloadEntry> GetDownloadsEntries(string baseRepository, string operatingSystem,
            bool is64BitArchitecture)
        {
            List<DownloadEntry> entries = new List<DownloadEntry>();

            if (DownloadInfo != null) {
                foreach (DownloadEntry downloadEntry in DownloadInfo.GetDownloadsEntries(
                    IsNatives && operatingSystem != null && (Natives?.ContainsKey(operatingSystem) ?? false)
                        ? Natives[operatingSystem].Replace("${arch}", is64BitArchitecture ? "64" : "32")
                        : null)) {
                    if (downloadEntry.Path == null) {
                        downloadEntry.Path = GetPath(operatingSystem, is64BitArchitecture);
                    }

                    entries.Add(downloadEntry);
                }
            } else {
                entries.Add(new DownloadEntry {
                    Url = (Url ?? baseRepository) + GetPath(operatingSystem, is64BitArchitecture), Path = GetPath(),
                    IsNatives = IsNatives
                });
            }

            return entries;
        }
    }
}
