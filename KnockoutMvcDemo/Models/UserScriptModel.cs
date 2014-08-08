using System;

namespace KnockoutMvcDemo.Models
{
    public class UserScriptModel
    {
        public string Message { get; set; }

        public void AddLetter()
        {
            Message += (char)('a' + new Random().Next(26));
        }
    }
}