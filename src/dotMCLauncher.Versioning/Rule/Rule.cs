using Newtonsoft.Json;

namespace dotMCLauncher.Versioning
{
    public class Rule : RuleConditions
    {
        public enum Actions
        {
            ALLOW,
            DISALLOW
        }

        [JsonProperty("action")]
        private string _action { get; set; }

        public Actions Action
        {
            get => _action == "allow" ? Actions.ALLOW : Actions.DISALLOW;
            set => _action = value == Actions.ALLOW ? "allow" : "disallow";
        }

        public bool CheckIfMeetsConditions(RuleConditions conditions)
        {
            bool osFulfils = OsConditions?.CheckIfMeetsConditions(conditions?.OsConditions) ?? true;

            bool featuresFulfil = FeatureConditions?.CheckIfMeetsConditions(conditions?.FeatureConditions) ?? true;

            return osFulfils && featuresFulfil;
        }
    }
}
