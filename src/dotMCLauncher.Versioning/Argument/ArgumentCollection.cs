using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dotMCLauncher.Versioning
{
    public class ArgumentCollection : Dictionary<string, List<string>>
    {
        public string SourceLine { get; private set; }
        private string _targetLine { get; set; }

        public ArgumentCollection(string sourceLine)
        {
            SourceLine = sourceLine;
            Parse();
        }

        /// <summary>
        /// Parses source argument line.
        /// </summary>
        public void Parse()
        {
            if (string.IsNullOrEmpty(SourceLine)) {
                throw new ArgumentNullException(nameof(SourceLine));
            }

            Regex re = new Regex(@"\-\-(\w+) (\S+)", RegexOptions.IgnoreCase);
            MatchCollection match = re.Matches(SourceLine);
            for (int i = 0; i < match.Count; i++) {
                if (!ContainsKey(match[i].Groups[1].Value)) {
                    Add(match[i].Groups[1].Value, new List<string> {
                        match[i].Groups[2].Value
                    });
                } else {
                    base[match[i].Groups[1].Value].Add(match[i].Groups[2].Value);
                }
            }

            _targetLine = re.Replace(SourceLine, string.Empty).Trim();
        }

        /// <summary>
        /// Parses source argument line from provided string.
        /// </summary>
        /// <param name="argLine">Input string.</param>
        public void Parse(string argLine)
        {
            SourceLine = argLine;
            Parse();
        }

        /// <summary>
        /// Adds argument into collection.
        /// </summary>
        /// <param name="key">Switch. (Key without argument)</param>
        public void Add(string key)
        {
            Add(key, new List<string>());
        }

        /// <summary>
        /// Adds argument into collection.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(string key, string value)
        {
            if (ContainsKey(key)) {
                base[key].Add(value);
                return;
            }

            Add(key, new List<string> {
                value
            });
        }

        /// <summary>
        /// Adds argument into collection.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="values">Values.</param>
        public void Add(string key, params string[] values)
        {
            if (!ContainsKey(key)) {
                Add(key, new List<string>());
            }

            foreach (string str in values) {
                base[key].Add(str);
            }
        }

        /// <summary>
        /// Builds argument line with replaced values.
        /// </summary>
        public override string ToString()
            => ToString(null);

        /// <summary>
        /// Builds argument line with replaced values.
        /// </summary>
        /// <param name="values">List of replaceable values.</param>
        public string ToString(Dictionary<string, string> values)
        {
            Regex re = new Regex(@"\$\{(\w+)\}", RegexOptions.IgnoreCase);
            StringBuilder toReturn = new StringBuilder();
            if (!string.IsNullOrEmpty(_targetLine)) {
                toReturn.Append(re.Replace(_targetLine,
                                    match => values != null && values.ContainsKey(match.Groups[1].Value)
                                        ? (!values[match.Groups[1].Value].Contains(' ')
                                            ? values[match.Groups[1].Value]
                                            : $"\"{values[match.Groups[1].Value]}\"")
                                        : match.Value) + " ");
            }

            foreach (string key in Keys) {
                string value;
                if (base[key]?.Count > 0 && values != null) {
                    foreach (string str in base[key]) {
                        value = str;
                        if (re.IsMatch(str)) {
                            value = re.Replace(str,
                                match =>
                                    values.ContainsKey(match.Groups[1].Value)
                                        ? values[match.Groups[1].Value]
                                        : str);
                        }

                        if (value.Contains(' ')) {
                            value = $"\"{value}\"";
                        }

                        if (value != string.Empty) {
                            value = " " + value;
                        }

                        toReturn.Append($"--{key}{value} ");
                    }
                } else if (base[key]?.Count > 0) {
                    foreach (string str in base[key]) {
                        value = str;
                        if (value.Contains(' ')) {
                            value = $"\"{value}\"";
                        }

                        if (value != string.Empty) {
                            value = " " + value;
                        }

                        toReturn.Append($"--{key}{value} ");
                    }
                } else {
                    toReturn.Append($"--{key} ");
                }
            }

            return toReturn.ToString().Substring(0, toReturn.Length - 1);
        }
    }
}
