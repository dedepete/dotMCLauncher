namespace dotMCLauncher.Yggdrasil
{
    public static class UrlProvider
    {
        public static string AuthServer { get; set; } = @"https://authserver.mojang.com";

        public static string AuthenticateUrl => AuthServer + @"/authenticate";
        public static string RefreshUrl => AuthServer + @"/refresh";
        public static string ValidateUrl => AuthServer + @"/validate";
        public static string SignoutUrl => AuthServer + @"/signout";
        public static string InvalidateUrl => AuthServer + @"/invalidate";
    }
}
