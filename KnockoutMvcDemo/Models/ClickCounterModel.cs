using DelegateDecompiler;

namespace KnockoutMvcDemo.Models
{
  public class ClickCounterModel
  {
    public int NumberOfClicks { get; set; }

    [Computed]
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