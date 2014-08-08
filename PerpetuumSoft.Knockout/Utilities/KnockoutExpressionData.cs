using System.Collections.Generic;

namespace PerpetuumSoft.Knockout
{
    public class KnockoutExpressionData
    {
        public KnockoutExpressionData()
        {
            InstanceNames = new[] { "" };
            Aliases = new Dictionary<string, string>();
        }

        public string[] InstanceNames { get; set; }
        public Dictionary<string, string> Aliases { get; set; }
        public bool NeedBracketsForAllMembers { get; set; }

        public static KnockoutExpressionData CreateConstructorData()
        {
            return new KnockoutExpressionData { InstanceNames = new[] { "this" }, NeedBracketsForAllMembers = true };
        }

        public KnockoutExpressionData Clone()
        {
            var data = new KnockoutExpressionData();
            if (InstanceNames != null)
                data.InstanceNames = InstanceNames.Clone() as string[];
            if (Aliases != null)
                foreach (var pair in Aliases)
                    data.Aliases[pair.Key] = pair.Value;
            data.NeedBracketsForAllMembers = NeedBracketsForAllMembers;
            return data;
        }
    }
}