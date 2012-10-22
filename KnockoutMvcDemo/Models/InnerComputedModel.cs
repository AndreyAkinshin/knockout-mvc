using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KnockoutMvcDemo.Models
{
  public class InnerComputedItemModel
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Expression<Func<string>> FullName()
    {
      return () => FirstName + " " + LastName;
    }
  }

  public class InnerComputedSubModel
  {
    public string Caption { get; set; }
    public int Value { get; set; }

    public Expression<Func<string>> Message()
    {
      return () => Caption + " = " + Value;
    }
  }

  public class InnerComputedModel
  {
    public List<InnerComputedItemModel> Items { get; set; }
    public InnerComputedSubModel SubModel { get; set; }
  }
}