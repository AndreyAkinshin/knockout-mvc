using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class BetterListModel
    {
        public string ItemToAdd { get; set; }
        public List<string> AllItems { get; set; }
        public List<string> SelectedItems { get; set; }

        public void AddItem()
        {
            if (!string.IsNullOrEmpty(ItemToAdd) && !AllItems.Contains(ItemToAdd))
                AllItems.Add(ItemToAdd);
            ItemToAdd = "";
        }

        public void RemoveSelected()
        {
            AllItems.RemoveAll(item => SelectedItems.Contains(item));
            SelectedItems.Clear();
        }

        public void SortItems()
        {
            AllItems.Sort();
        }
    }
}