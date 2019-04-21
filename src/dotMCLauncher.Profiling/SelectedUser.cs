using Newtonsoft.Json;

namespace dotMCLauncher.Profiling
{
    public class SelectedUser : JsonSerializable
    {
        [JsonProperty("account")]
        public string SelectedGuid { get; set; }

        [JsonProperty("profile")]
        public string SelectedProfile { get; set; }
    }
}
