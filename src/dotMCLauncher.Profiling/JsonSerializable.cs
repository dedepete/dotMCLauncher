using Newtonsoft.Json;

namespace dotMCLauncher.Profiling
{
    public abstract class JsonSerializable
    {
        public string ToJson()
        {
            return ToJson(Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public string ToJson(Formatting formatting)
        {
            return ToJson(formatting, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public string ToJson(Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(this, formatting, settings);
        }
    }
}
