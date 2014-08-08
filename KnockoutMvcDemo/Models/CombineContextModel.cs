using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class CombineContextSubItemModel
    {
        public string Name { get; set; }
    }

    public class CombineContextItemModel
    {
        public string Caption { get; set; }
        public List<CombineContextSubItemModel> SubItems { get; set; }
    }

    public class CombineContextModel
    {
        public string Key { get; set; }
        public List<CombineContextItemModel> Items { get; set; }
    }
}