using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Versioning
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class VersionManifest : BaseVersion
    {
        [JsonIgnore]
        public VersionManifestType Type { get; set; } = VersionManifestType.V1;

        /// <summary>
        /// Arguments. v1
        /// </summary>
        public string MinecraftArguments
        {
            get => _arguments;
            set {
                _arguments = value;
                ArgumentsCollection = new ArgumentCollection();
                ArgumentsCollection.Parse(value);
            }
        }

        /// <summary>
        /// Arguments. v2
        /// </summary>
        public JObject Arguments
        {
            get => ArgumentsGroups != null ? JObject.FromObject(ArgumentsGroups) : null;
            set {
                Type = VersionManifestType.V2;
                ArgumentsGroups = new List<ArgumentsGroup>();
                foreach (KeyValuePair<string, JToken> pair in value) {
                    ArgumentsGroup group = new ArgumentsGroup();
                    group.Type = "game".Equals(pair.Key, StringComparison.OrdinalIgnoreCase)
                        ? ArgumentsGroupType.GAME
                        : ArgumentsGroupType.JVM;
                    group.Arguments = new List<Argument>();
                    JArray array = pair.Value as JArray;
                    foreach (JToken token in array) {
                        if (token is JValue) {
                            group.Arguments.Add(new SingleArgument(token));
                        } else {
                            group.Arguments.Add(token.ToObject<ExtendedArgument>());
                        }
                    }

                    ArgumentsGroups.Add(group);
                }
            }
        }

        /// <summary>
        /// Assets ID.
        /// </summary>
        public string Assets { get; set; }

        public DownloadEntry AssetIndex { get; set; }

        /// <summary>
        /// Main class.
        /// </summary>
        public string MainClass { get; set; }

        /// <summary>
        /// Library list.
        /// </summary>
        public List<Library> Libraries { get; set; }

        /// <summary>
        /// Release date and time.
        /// </summary>
        public string ReleaseTime { get; set; }

        /// <summary>
        /// Date and time of last update.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Parent's ID.
        /// </summary>
        public string InheritsFrom { get; set; }

        /// <summary>
        /// Version download information. See <see cref="VersionDownloadInfo"/>.
        /// </summary>
        public VersionDownloadInfo Downloads { get; set; }

        /// <summary>
        /// Parent's manifest.
        /// </summary>
        [JsonIgnore]
        public VersionManifest InheritableVersionManifest { get; set; }

        /// <summary>
        /// Argument line. v1
        /// </summary>
        [JsonIgnore]
        private string _arguments { get; set; }

        /// <summary>
        /// Argument list. v1
        /// </summary>
        [JsonIgnore]
        public ArgumentCollection ArgumentsCollection { get; set; }

        /// <summary>
        /// Groups of arguments. v2
        /// </summary>
        [JsonIgnore]
        public List<ArgumentsGroup> ArgumentsGroups { get; set; }

        /// <summary>
        /// Parses build's JSON file.
        /// </summary>
        /// <param name="pathToDirectory">Path to build's directory.</param>
        public static VersionManifest ParseFromDirectory(DirectoryInfo pathToDirectory) => ParseFromDirectory(pathToDirectory, true);

        /// <summary>
        /// Parses build's JSON file.
        /// </summary>
        /// <param name="pathToDirectory">Path to build's directory.</param>
        /// <param name="parseInheritableVersion">Parses inheritable builds.</param>
        public static VersionManifest ParseFromDirectory(DirectoryInfo pathToDirectory, bool parseInheritableVersion)
        {
            IsValid(pathToDirectory, true);
            string version = pathToDirectory.Name;
            VersionManifest ver = JsonConvert.DeserializeObject(
                File.ReadAllText(Path.Combine(pathToDirectory.FullName, version + ".json")),
                typeof(VersionManifest)) as VersionManifest;
            if (ver.InheritsFrom == null || !parseInheritableVersion) {
                return ver;
            }

            ver.InheritableVersionManifest =
                ParseFromDirectory(new DirectoryInfo(Path.Combine(pathToDirectory.Parent.FullName, ver.InheritsFrom)));
            ver.Libraries.AddRange(ver.InheritableVersionManifest.Libraries);
            return ver;
        }

        public static VersionManifest Parse(string content)
        {
            return JsonConvert.DeserializeObject(content, typeof(VersionManifest)) as VersionManifest;
        }

        public static bool IsValid(string pathToDirectory) => IsValid(pathToDirectory, false);

        public static bool IsValid(string pathToDirectory, bool throwsExceptions) => IsValid(new DirectoryInfo(pathToDirectory), false);

        public static bool IsValid(DirectoryInfo directoryInfo) => IsValid(directoryInfo, false);

        public static bool IsValid(DirectoryInfo pathToDirectory, bool throwsExceptions)
        {
            string version = pathToDirectory.Name;
            string jsonPath = Path.Combine(pathToDirectory.FullName, version + ".json");

            if (!File.Exists(jsonPath)) {
                if (throwsExceptions) {
                    throw new VersionNotFound("Unable to locate JSON file.",
                        new FileNotFoundException($"The following file is required to parse version's manifest: '{jsonPath}'",
                            jsonPath));
                }

                return false;
            }

            if (JsonConvert.DeserializeObject(File.ReadAllText(jsonPath), typeof(VersionManifest)) is VersionManifest) {
                return true;
            }

            if (throwsExceptions) {
                throw new VersionCorrupted("Unable to load JSON file.",
                    new FileLoadException($"The following file is corrupted and cannot be loaded: {jsonPath}", jsonPath));
            }

            return false;
        }

        public bool TryToValidate(DirectoryInfo pathToDirectory)
        {
            string version = pathToDirectory.Name;
            if (!File.Exists(Path.Combine(pathToDirectory.ToString(), version + ".json"))) {
                return false;
            }

            VersionManifest versionManifest =
                (VersionManifest) JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(pathToDirectory.ToString(), version + ".json")),
                    typeof(VersionManifest));
            return versionManifest != null;
        }

        public string GetBaseAssetsId()
        {
            if (!string.IsNullOrEmpty(Assets)) {
                return Assets;
            }

            VersionManifest manifest = InheritableVersionManifest;
            while (true) {
                if (manifest?.InheritsFrom == null) {
                    if (manifest?.Assets != null) {
                        return manifest.Assets;
                    }

                    break;
                }

                manifest = manifest.InheritableVersionManifest;
            }

            return "legacy";
        }

        public DownloadEntry GetBaseAssetIndex()
        {
            if (AssetIndex != null) {
                return AssetIndex;
            }

            VersionManifest manifest = InheritableVersionManifest;
            while (true) {
                if (manifest?.InheritsFrom == null) {
                    if (manifest?.Assets != null) {
                        return manifest.AssetIndex;
                    }

                    break;
                }

                manifest = manifest.InheritableVersionManifest;
            }

            return null;
        }

        public string GetBaseJar()
        {
            return InheritsFrom == null ? VersionId : InheritableVersionManifest.GetBaseJar();
        }

        public string BuildArgumentsByGroup(ArgumentsGroupType group, Dictionary<string, string> jvmArgumentDictionary, RuleConditions conditions)
        {
            string toReturn = ArgumentsGroups?.FirstOrDefault(ag => ag.Type == group)?
                                  .ToString(jvmArgumentDictionary, conditions) ?? string.Empty;
            if (InheritableVersionManifest != null && InheritableVersionManifest.Type == VersionManifestType.V2) {
                toReturn = (toReturn == string.Empty ? string.Empty : toReturn + " ") +
                           InheritableVersionManifest.BuildArgumentsByGroup(group, jvmArgumentDictionary, conditions);
            }

            return toReturn;
        }

        public string GetClientDownloadUrl()
            =>
                Downloads?.Client?.Url ??
                $@"https://s3.amazonaws.com/Minecraft.Download/versions/{VersionId}/{VersionId}.jar";

        public string GetAssetsIndexDownloadUrl()
            => GetBaseAssetIndex()?.Url ?? $"https://s3.amazonaws.com/Minecraft.Download/indexes/{Assets ?? "legacy"}.json";
    }

    public class VersionNotFound : Exception
    {
        public VersionNotFound(string message, Exception innerException) : base(message, innerException) { }
    }

    public class VersionCorrupted : Exception
    {
        public VersionCorrupted(string message, Exception innerException) : base(message, innerException) { }
    }
}
