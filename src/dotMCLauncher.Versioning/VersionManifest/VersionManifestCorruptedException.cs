using System;

namespace dotMCLauncher.Versioning {
    public class VersionManifestCorruptedException : Exception
    {
        public string VersionId { get; }
        public VersionManifestCorruptedException(string message) : base(message) { }

        public VersionManifestCorruptedException(string message, string versionId) : base(message)
        {
            VersionId = versionId;
        }

        public VersionManifestCorruptedException(string message, Exception innerException) : base(message, innerException) { }

        public VersionManifestCorruptedException(string message, Exception innerException, string versionId) : base(message,
            innerException)
        {
            VersionId = versionId;
        }
    }
}
