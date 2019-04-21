using System.Collections;
using System.Collections.Generic;

namespace dotMCLauncher.Versioning
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RuleCollection : ICollection<Rule>
    {
        private readonly List<Rule> _rules;

        public RuleCollection()
        {
            _rules = new List<Rule>();
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            return _rules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Rule rule)
        {
            _rules.Add(rule);
        }

        public void Clear()
        {
            _rules.Clear();
        }

        public bool Contains(Rule rule)
        {
            return _rules.Contains(rule);
        }

        public void CopyTo(Rule[] arrayOfRules, int arrayIndex)
        {
            _rules.CopyTo(arrayOfRules, arrayIndex);
        }

        public bool Remove(Rule rule)
        {
            return _rules.Remove(rule);
        }

        public int Count => _rules.Count;

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public bool IsReadOnly { get; }

        public int IndexOf(Rule rule)
        {
            return _rules.IndexOf(rule);
        }

        public void Insert(int index, Rule rule)
        {
            _rules.Insert(index, rule);
        }

        public void RemoveAt(int index)
        {
            _rules.RemoveAt(index);
        }

        public Rule this[int index]
        {
            get => _rules[index];
            set => _rules[index] = value;
        }

        public bool CheckIfAllowed(RuleConditions ruleConditions)
        {
            if (_rules == null || Count == 0) {
                return true;
            }

            if (Count == 1 && _rules[0].CheckIfMeetsConditions(ruleConditions)) {
                return _rules[0].Action == Rule.Actions.ALLOW;
            }

            foreach (Rule rule in _rules) {
                if (rule.Action == Rule.Actions.ALLOW && !rule.CheckIfMeetsConditions(ruleConditions)) {
                    return false;
                }

                if (rule.Action == Rule.Actions.DISALLOW && rule.CheckIfMeetsConditions(ruleConditions)) {
                    return false;
                }
            }

            return true;
        }
    }
}
