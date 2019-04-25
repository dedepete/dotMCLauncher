using Newtonsoft.Json;

namespace dotMCLauncher.Profiling
{
    public abstract class JsonSerializable
    {
        public string ToJson()
            => ToJson(Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore,  DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

        public string ToJson(Formatting formatting)
            => ToJson(formatting, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

        public string ToJson(Formatting formatting, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(this, formatting, settings);
    }
}
