using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class CollectionsPersonModel
    {
        public string Name { get; set; }
        public List<string> Children { get; set; }
    }

    public class CollectionsModel
    {
        public bool ShowRenderTime { get; set; }
        public List<CollectionsPersonModel> People { get; set; }

        public void AddChild(int index)
        {
            if (index >= 0 && index < People.Count)
                People[index].Children.Add("New child");
        }
    }
}