namespace dotMCLauncher.Yggdrasil
{
    public abstract class ExceptionResponse
    {
        public string Error { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public string Cause { get; protected set; }
    }
}
