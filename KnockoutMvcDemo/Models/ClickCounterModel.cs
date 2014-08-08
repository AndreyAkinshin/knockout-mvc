using System.Web.Script.Serialization;
using DelegateDecompiler;
using Newtonsoft.Json;

namespace KnockoutMvcDemo.Models
{
  public class ClickCounterModel
  {
    public int NumberOfClicks { get; set; }

    [Computed]
    [ScriptIgnore]
    [JsonIgnore]
    public bool HasClickedTooManyTimes
    {
      get { return NumberOfClicks >= 3; }
    }

    public void RegisterClick()
    {
      NumberOfClicks++;
    }

    public void ResetClicks()
    {
      NumberOfClicks = 0;
    }
  }
}