using System;

namespace dotMCLauncher.Versioning {
    public class VersionManifestParentParseException : Exception
    {
        public string VersionId { get; }
        public string BaseVersionId { get; }

        public VersionManifestParentParseException(Exception innerException, string versionId, string baseVersionId) : base(
            "Unable to parse parent version manifest.",
            innerException)
        {
            VersionId = versionId;
            BaseVersionId = baseVersionId;
        }

        public VersionManifestParentParseException(Exception innerException) : base("Unable to parse parent version.",
            innerException) { }
    }
}
