using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model
{
    public static class Utils
    {
        //public static RulesChercker<int> LeftTopNeighbor => new RulesChercker<int>();
        //public static RulesChercker<int> LeftBotNeighbor => new RulesChercker<int>();
        //public static RulesChercker<int> RightTopNeighbor => new RulesChercker<int>();
        //public static RulesChercker<int> RightBotNeighbor => new RulesChercker<int>();
    }

    public class RulesChercker<T>
    {
        private readonly IReadOnlyCollection<Func<T, bool>> _rules;

        public RulesChercker(IReadOnlyCollection<Func<T, bool>> rules) => _rules = rules;

        public bool CherckRules(T value, out T result)
        {
            result = value;
            return _rules.All(x => x?.Invoke(value) ?? true);
        }
    }

    public class GameRules
    {

        //public FindAvaliableStep()
        //{

        //}
    }
}
