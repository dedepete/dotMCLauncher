using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Yggdrasil
{
    public interface IAuthenticateResponse
    {
        [JsonProperty("accessToken")]
        string AccessToken { get; set; }

        [JsonProperty("clientToken")]
        string ClientToken { get; set; }

        [JsonProperty("selectedProfile")]
        JObject SelectedProfile { get; set; }

        [JsonProperty("user")]
        JObject User { get; set; }

        [JsonIgnore]
        string Uuid { get; }
    }
}
