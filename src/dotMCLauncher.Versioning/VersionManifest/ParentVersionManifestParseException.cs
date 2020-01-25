using System;

namespace dotMCLauncher.Versioning {
    public class ParentVersionManifestParseException : Exception
    {
        public string VersionId { get; }
        public string BaseVersionId { get; }

        public ParentVersionManifestParseException(Exception innerException, string versionId, string baseVersionId) : base(
            "Unable to parse parent version manifest.",
            innerException)
        {
            VersionId = versionId;
            BaseVersionId = baseVersionId;
        }

        public ParentVersionManifestParseException(Exception innerException) : base("Unable to parse parent version.",
            innerException) { }
    }
}
