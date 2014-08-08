using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class RegionSubSubModel
    {
        public string SubSubModelName { get; set; }
    }

    public class RegionSubModel
    {
        public string SubModelName { get; set; }
        public RegionSubSubModel SubSubModel { get; set; }
    }

    public class RegionModel
    {
        public bool Condition1 { get; set; }
        public bool Condition2 { get; set; }
        public List<string> Items { get; set; }
        public string ModelName { get; set; }
        public RegionSubModel SubModel { get; set; }
    }
}