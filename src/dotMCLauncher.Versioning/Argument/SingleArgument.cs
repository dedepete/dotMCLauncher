using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Versioning
{
    public class SingleArgument : Argument
    {
        public SingleArgument(JToken value)
        {
            Type = ArgumentType.SINGLE;
            Value = value;
        }

        public JToken Value { get; }
    }
}
