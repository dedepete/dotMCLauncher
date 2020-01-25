using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class RawVersionListManifest
    {
        [JsonProperty("latest")]
        public RawVersionListManifestLatest LatestVersions { get; set; }

        [JsonProperty("versions")]
        public List<RawVersionListManifestEntry> Versions { get; set; }

        public List<RawVersionListManifestEntry> GetVersionsByType(string type)
            => GetVersionsByType(type, RawVersionListManifestSortMethod.INCLUDE);

        public List<RawVersionListManifestEntry> GetVersionsByType(string type,
            RawVersionListManifestSortMethod sorting)
            => Versions.Where(x => sorting == RawVersionListManifestSortMethod.INCLUDE
                                  ? x.ReleaseType == type
                                  : x.ReleaseType != type).ToList();

        public List<RawVersionListManifestEntry> GetVersionsByTypes(string[] types)
            => GetVersionsByTypes(types, RawVersionListManifestSortMethod.INCLUDE);

        public List<RawVersionListManifestEntry> GetVersionsByTypes(string[] types,
            RawVersionListManifestSortMethod sorting)
            => Versions.Where(x => sorting == RawVersionListManifestSortMethod.INCLUDE
                                  ? types.Contains(x.ReleaseType)
                                  : !types.Contains(x.ReleaseType)).ToList();

        public RawVersionListManifestEntry GetVersion(string version)
            => Versions.Count(x => x.VersionId == version) == 1
                ? Versions.Where(x => x.VersionId == version).ToArray()[0]
                : null;

        public static RawVersionListManifest ParseList(string content)
            => (RawVersionListManifest)
                JsonConvert.DeserializeObject(content, typeof(RawVersionListManifest));

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public enum RawVersionListManifestSortMethod
    {
        INCLUDE,
        EXCLUDE
    }
}
