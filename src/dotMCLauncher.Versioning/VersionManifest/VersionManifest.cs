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
        public VersionManifestType Type
        {
            get {
                if (Arguments != null) {
                    return VersionManifestType.V2;
                }

                if (InheritableVersionManifest != null) {
                    return InheritableVersionManifest.Type;
                }

                return ArgumentsCollection != null ? VersionManifestType.LEGACY : VersionManifestType.V2;
            }
        }

        /// <summary>
        /// Arguments. Legacy
        /// </summary>
        public string MinecraftArguments
        {
            get => ArgumentsCollection.SourceLine;
            set => ArgumentsCollection = new ArgumentCollection(value);
        }

        /// <summary>
        /// Arguments. v2
        /// </summary>
        public JObject Arguments
        {
            get {
                if (ArgumentsGroups == null || ArgumentsGroups.Count == 0) {
                    return null;
                }

                JObject jObject = new JObject();
                foreach (ArgumentsGroup argumentsGroup in ArgumentsGroups) {
                    jObject.Add(argumentsGroup.Type.ToString().ToLowerInvariant(), JArray.FromObject(argumentsGroup.Arguments));
                }

                return jObject;
            }
            set {
                ArgumentsGroups = new List<ArgumentsGroup>();
                foreach (KeyValuePair<string, JToken> pair in value) {
                    ArgumentsGroup group = new ArgumentsGroup();
                    group.Type = "game".Equals(pair.Key, StringComparison.OrdinalIgnoreCase)
                        ? ArgumentsGroupType.GAME
                        : ArgumentsGroupType.JVM;
                    group.Arguments = new List<Argument>();
                    if (pair.Value is JArray array) {
                        foreach (JToken token in array) {
                            if (token is JValue) {
                                group.Arguments.Add(new SingleArgument(token));
                            } else {
                                group.Arguments.Add(token.ToObject<MultipleArgument>());
                            }
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
        /// The version of the required Java Runtime Environment. See <see cref="VersionManifestJavaVersion"/>.
        /// </summary>
        public VersionManifestJavaVersion JavaVersion { get; set; }

        /// <summary>
        /// Its value is 1 for all recent versions of the game or 0 for all others. This tag tells the launcher whether it should urge the user to be careful since this version is older and might not support the latest player safety features.
        /// </summary>
        public int ComplianceLevel { get; set; }

        /// <summary>
        /// Parent's manifest.
        /// </summary>
        [JsonIgnore]
        public VersionManifest InheritableVersionManifest { get; set; }

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
        public static VersionManifest ParseFromDirectory(DirectoryInfo pathToDirectory)
            => ParseFromDirectory(pathToDirectory, true);

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
            if (ver?.InheritsFrom == null || !parseInheritableVersion) {
                return ver;
            }

            try {
                ver.InheritableVersionManifest =
                    ParseFromDirectory(
                        new DirectoryInfo(Path.Combine(pathToDirectory.Parent?.FullName ?? throw new DirectoryNotFoundException(), ver.InheritsFrom)));
                ver.Libraries.AddRange(ver.InheritableVersionManifest.Libraries);
            } catch (Exception exception) {
                throw new VersionManifestParentParseException(exception, ver.VersionId, ver.InheritsFrom);
            }

            return ver;
        }

        public static VersionManifest Parse(string content)
        {
            return JsonConvert.DeserializeObject(content, typeof(VersionManifest)) as VersionManifest;
        }

        public static bool IsValid(string pathToDirectory) => IsValid(pathToDirectory, false);

        public static bool IsValid(string pathToDirectory, bool throwsExceptions)
            => IsValid(new DirectoryInfo(pathToDirectory), throwsExceptions);

        public static bool IsValid(DirectoryInfo directoryInfo) => IsValid(directoryInfo, false);

        public static bool IsValid(DirectoryInfo pathToDirectory, bool throwsExceptions)
        {
            string version = pathToDirectory.Name;
            string jsonPath = Path.Combine(pathToDirectory.FullName, version + ".json");

            if (!File.Exists(jsonPath)) {
                if (throwsExceptions) {
                    throw new VersionManifestNotFoundException("Unable to locate JSON file.",
                        new FileNotFoundException(
                            $"The following file is required to parse version's manifest: '{jsonPath}'",
                            jsonPath), version);
                }

                return false;
            }

            try {
                if (Parse(File.ReadAllText(jsonPath)) != null) {
                    return true;
                }
            }
            catch (Exception exception) {
                if (throwsExceptions) {
                    throw new VersionManifestCorruptedException("Unable to load JSON file.",
                        new FileLoadException($"The following file is corrupted and cannot be loaded: {jsonPath}",
                            jsonPath, exception), version);
                }
            }

            if (throwsExceptions) {
                throw new VersionManifestCorruptedException("Unable to load JSON file.",
                    new FileLoadException($"The following file is corrupted and cannot be loaded: {jsonPath}",
                        jsonPath), version);
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
                (VersionManifest) JsonConvert.DeserializeObject(
                    File.ReadAllText(Path.Combine(pathToDirectory.ToString(), version + ".json")),
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

        public VersionManifestJavaVersion GetJavaVersion()
        {
            return InheritsFrom == null ? JavaVersion : InheritableVersionManifest.GetJavaVersion();
        }

        public string BuildArgumentsByGroup(ArgumentsGroupType group, Dictionary<string, string> jvmArgumentDictionary,
            RuleConditions conditions)
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
            => GetBaseAssetIndex()?.Url ??
               $"https://s3.amazonaws.com/Minecraft.Download/indexes/{Assets ?? "legacy"}.json";
    }
}
