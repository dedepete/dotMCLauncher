using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dotMCLauncher.Versioning
{
    public class ArgumentsGroup
    {
        public ArgumentsGroupType Type { get; set; }

        public List<Argument> Arguments { get; set; }

        public string ToString(Dictionary<string, string> values, RuleConditions conditions = null)
        {
            StringBuilder toReturn = new StringBuilder();
            foreach (Argument argument in Arguments) {
                switch (argument.Type) {
                    case ArgumentType.SINGLE:
                        toReturn.Append((argument as SingleArgument).Value + " ");
                        break;
                    case ArgumentType.EXTENDED:
                        ExtendedArgument extendedArgument = argument as ExtendedArgument;
                        if (!extendedArgument.IsAllowed(conditions)) {
                            continue;
                        }

                        if (!extendedArgument.HasMultipleArguments) {
                            toReturn.Append((extendedArgument.Value.Contains(' ')
                                                ? "\"" + extendedArgument.Value + "\""
                                                : extendedArgument.Value) + " ");
                            continue;
                        }

                        toReturn = extendedArgument.Values.Aggregate(toReturn,
                            (current, value) =>
                                toReturn.Append((Type == ArgumentsGroupType.JVM && value.Contains(' ')
                                                    ? "\"" + value + "\""
                                                    : value) + " "));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Regex re = new Regex(@"\$\{(\w+)\}", RegexOptions.IgnoreCase);

            if (toReturn.Length > 0) {
                toReturn.Length--;
            }

            foreach (Match match in re.Matches(toReturn.ToString())) {
                if (values?.ContainsKey(match.Groups[1].Value) == true && values[match.Groups[1].Value] != null) {
                    toReturn.Replace(match.Value, !values[match.Groups[1].Value].Contains(' ')
                        ? values[match.Groups[1].Value]
                        : $"\"{values[match.Groups[1].Value]}\"");
                } else {
                    toReturn.Replace(match.Value, match.Groups[1].Value);
                }
            }

            return toReturn.ToString();
        }
    }
}
