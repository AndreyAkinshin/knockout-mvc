using DelegateDecompiler;

namespace KnockoutMvcDemo.Models
{
  public class HelloWorldModel
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Computed]
    public string FullName
    {
      get { return FirstName + " " + LastName; }
    }
  }
}