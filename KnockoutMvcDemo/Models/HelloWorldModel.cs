using System;
using System.Linq.Expressions;

namespace KnockoutMvcDemo.Models
{
  public class HelloWorldModel
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Expression<Func<string>> FullName()
    {
      return () => FirstName + " " + LastName;
    }
  }
}