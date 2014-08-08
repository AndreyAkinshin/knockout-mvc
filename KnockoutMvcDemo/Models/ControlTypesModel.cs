using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
    public class ControlTypesModel
    {
        public string StringValue { get; set; }
        public string PasswordValue { get; set; }
        public bool BooleanValue { get; set; }
        public List<string> OptionValue { get; set; }
        public string SelectedOptionValue { get; set; }
        public List<string> MultipleSelectedOptionValues { get; set; }
        public string RadioSelectedOptionValue { get; set; }
    }
}