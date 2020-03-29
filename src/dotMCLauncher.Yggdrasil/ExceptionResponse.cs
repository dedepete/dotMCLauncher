namespace dotMCLauncher.Yggdrasil
{
    public abstract class ExceptionResponse
    {
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Cause { get; set; }
    }
}
