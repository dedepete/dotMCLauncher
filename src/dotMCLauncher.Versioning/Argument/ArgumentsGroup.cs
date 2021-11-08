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
                        break;
                    case ArgumentType.MULTIPLE:
                        if (!(argument is MultipleArgument multipleArgument)) {
                            break;
                        }

                        if (!multipleArgument.IsAllowed(conditions)) {
                            continue;
                        }

                        if (multipleArgument.HasMultipleArguments) {
                            toReturn = multipleArgument.Values.Aggregate(toReturn,
                            (current, value) =>
                                toReturn.Append((value.ToString().Contains(' ')
                                                    ? "\"" + value + "\""
                                                    : value) + " "));
                            continue;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                toReturn.Append((argument.Value.ToString().Contains(' ')
                                                ? "\"" + argument.Value + "\""
                                                : argument.Value) + " ");
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
