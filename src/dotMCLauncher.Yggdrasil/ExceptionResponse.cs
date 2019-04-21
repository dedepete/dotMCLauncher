using Newtonsoft.Json;

namespace dotMCLauncher.Yggdrasil
{
    public abstract class ExceptionResponse
    {
        [JsonIgnore]
        public bool IsErrored { get; set; }

        private string _error { get; set; }

        public string Error
        {
            get => _error;

            set {
                _error = value;
                IsErrored = true;
            }
        }

        private string _errorMessage { get; set; }

        public string ErrorMessage
        {
            get => _errorMessage;

            set {
                _errorMessage = value;
                IsErrored = true;
            }
        }

        private string _cause { get; set; }

        public string Cause
        {
            get => _cause;

            set {
                _cause = value;
                IsErrored = true;
            }
        }
    }
}
