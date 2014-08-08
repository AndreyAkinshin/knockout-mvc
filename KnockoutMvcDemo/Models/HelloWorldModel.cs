using System.Web.Script.Serialization;
using DelegateDecompiler;
using Newtonsoft.Json;

namespace KnockoutMvcDemo.Models
{
    public class HelloWorldModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Computed]
        [ScriptIgnore]
        [JsonIgnore]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}