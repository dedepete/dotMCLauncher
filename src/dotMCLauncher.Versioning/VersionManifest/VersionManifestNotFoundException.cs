using System;

namespace dotMCLauncher.Versioning {
    public class VersionManifestNotFoundException : Exception
    {
        public string VersionId { get; }
        public VersionManifestNotFoundException(string message) : base(message) { }

        public VersionManifestNotFoundException(string message, string versionId) : base(message)
        {
            VersionId = versionId;
        }

        public VersionManifestNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public VersionManifestNotFoundException(string message, Exception innerException, string versionId) : base(message,
            innerException)
        {
            VersionId = versionId;
        }
    }
}
