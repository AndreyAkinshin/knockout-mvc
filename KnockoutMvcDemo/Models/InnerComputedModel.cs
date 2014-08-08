using System.Collections.Generic;
using System.Web.Script.Serialization;
using DelegateDecompiler;
using Newtonsoft.Json;

namespace KnockoutMvcDemo.Models
{
  public class InnerComputedItemModel
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

  public class InnerComputedSubModel
  {
    public string Caption { get; set; }
    public int Value { get; set; }

    [Computed]
    [ScriptIgnore]
    [JsonIgnore]
    public string Message
    {
      get { return Caption + " = " + Value; }
    }
  }

  public class InnerComputedModel
  {
    public List<InnerComputedItemModel> Items { get; set; }
    public InnerComputedSubModel SubModel { get; set; }
  }
}