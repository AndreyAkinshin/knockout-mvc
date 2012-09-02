using System;
using System.Linq.Expressions;

namespace KnockoutMvcDemo.Models
{
  public class ClickCounterModel
  {
    public int NumberOfClicks { get; set; }

    public Expression<Func<bool>> HasClickedTooManyTimes()
    {
      return () => NumberOfClicks >= 3;
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