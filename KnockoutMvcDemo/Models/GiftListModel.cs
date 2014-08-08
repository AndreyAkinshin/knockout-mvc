using System;
using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class GiftListModel
    {
        public List<GiftModel> Gifts { get; set; }
        public string ServerTime { get; set; }

        public void AddGift()
        {
            Gifts.Add(new GiftModel());
        }

        public void RemoveGift(int index)
        {
            Gifts.RemoveAt(index);
        }

        public void Save()
        {
            ServerTime = DateTime.Now.ToString();
        }
    }

    public class GiftModel
    {
        public string Title { get; set; }
        public double Price { get; set; }
    }
}