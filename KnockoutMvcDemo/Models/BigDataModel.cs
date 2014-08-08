using System;
using System.Collections.Generic;
using System.Threading;

namespace KnockoutMvcDemo.Models
{
    public class BigDataModel
    {
        public string Key { get; set; }
        public List<int> Items { get; set; }

        public void LoadData()
        {
            Key = "Key " + new Random().Next();
            Items = new List<int>();
            for (int i = 0; i < 100; i++)
                Items.Add(i);

            Thread.Sleep(2000);
        }
    }
}