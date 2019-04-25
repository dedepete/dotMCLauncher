using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class FeatureConditions
    {
        [JsonProperty("has_custom_resolution")]
        public bool? IsForCustomResolution { get; set; }

        [JsonProperty("is_demo_user")]
        public bool? IsForDemoUser { get; set; }

        private Dictionary<string, bool?> ToDictionary()
        {
            Dictionary<string, bool?> toReturn = new Dictionary<string, bool?>();
            if (IsForDemoUser != null) {
                toReturn.Add("is_demo_user", IsForDemoUser);
            }

            if (IsForCustomResolution != null) {
                toReturn.Add("has_custom_resolution", IsForCustomResolution);
            }

            return toReturn;
        }

        public bool CheckIfMeetsConditions(FeatureConditions conditions)
        {
            if (ToDictionary().Count == 0) {
                return false;
            }

            if (conditions == null) {
                return false;
            }

            foreach (KeyValuePair<string, bool?> keyValue in ToDictionary()) {
                if (!conditions.ToDictionary().Contains(keyValue)) {
                    return false;
                }
            }

            return true;
        }
    }
}
