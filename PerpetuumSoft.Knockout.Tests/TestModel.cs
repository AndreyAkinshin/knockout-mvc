using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Script.Serialization;

namespace PerpetuumSoft.Knockout.Tests
{
  public class TestModel
  {
    public string A { get; set; }

    public string B { get; set; }

    public string C { get; set; }

    public bool Bool { get; set; }

    public List<string> List { get; set; }

    public List<int> IntList { get; set; }

    public TestModel SubModel { get; set; }

    [ScriptIgnore]
    public Expression<Func<string>> Concatenation
    {
      get
      {
        return () => "#" + A + B + C;
      }
    }
  }
}