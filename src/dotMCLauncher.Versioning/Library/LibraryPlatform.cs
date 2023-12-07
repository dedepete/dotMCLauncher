namespace dotMCLauncher.Versioning
{
    public static class LibraryPlatform
    {
        public enum OperatingSystem
        {
            WINDOWS,
            LINUX,
            MACOS,
            UNKNOWN
        }

        public enum Architecture
        {
            X86,
            X64,
            ARM64,
            UNKNOWN
        }

        public static OperatingSystem GetOperatingSystem(string operatingSystem)
        {
            switch (operatingSystem?.ToLowerInvariant()) {
                case "windows":
                    return OperatingSystem.WINDOWS;
                case "linux":
                    return OperatingSystem.LINUX;
                case "macos":
                    return OperatingSystem.MACOS;
                default:
                    return OperatingSystem.UNKNOWN;
            }
        }

        public static Architecture GetArchitecture(string architecture)
        {
            switch (architecture?.ToLowerInvariant()) {
                case "x86":
                    return Architecture.X86;
                case "x64":
                    return Architecture.X64;
                case "arm64":
                    return Architecture.ARM64;
                default:
                    return Architecture.UNKNOWN;
            }
        }

        public static string GetString(OperatingSystem operatingSystem)
        {
            switch (operatingSystem) {
                case OperatingSystem.WINDOWS:
                    return "windows";
                case OperatingSystem.LINUX:
                    return "linux";
                case OperatingSystem.MACOS:
                    return "macos";
                default:
                    return null;
            }
        }

        public static string GetString(Architecture architecture)
        {
            switch (architecture) {
                case Architecture.X86:
                    return "x86";
                case Architecture.X64:
                    return "x64";
                case Architecture.ARM64:
                    return "arm64";
                default:
                    return null;
            }
        }
    }
}
