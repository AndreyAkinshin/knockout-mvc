using System.IO;
using System.Web.Mvc;
using System;
using System.Linq.Expressions;
using PerpetuumSoft.Knockout;
using NUnit.Framework;


namespace PerpetuumSoft.Knockout.Tests
{
  [TestFixture]
  public class KnockoutExpressionConverterTest
  {
    private void AssertStringEquivalent(string a, string b)
    {
      a = a.Replace(" ", "");
      b = b.Replace(" ", "");
      Assert.AreEqual(a, b);
    }

    private void RunTest(Expression expression, string expected, KnockoutExpressionData data = null)
    {
      string actual = KnockoutExpressionConverter.Convert(expression, data);
      AssertStringEquivalent(expected, actual);
    }

    // Costructor
    [Test]
    public void ConstructorCommonTest1()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "this.A()", KnockoutExpressionData.CreateConstructorData());
    }

    [Test]
    public void ConstructorCommonTest2()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B), "(this.A() + this.B())", KnockoutExpressionData.CreateConstructorData());
    }

    [Test]
    public void ConstructorCommonTest3()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B + "!"), "((this.A() + this.B()) + '!')", KnockoutExpressionData.CreateConstructorData());
    }

    // Common
    [Test]
    public void CommonTest01()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "A");
    }

    [Test]
    public void CommonTest02()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B), "(A() + B())");
    }

    [Test]
    public void CommonTest03()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.Bool ? model.A : "Line"), "Bool() ? A() : 'Line'");
    }

    [Test]
    public void CommonTest04()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => true), "true");
    }

    [Test]
    public void CommonTest05()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => false), "false");
    }

    [Test]
    public void CommonTest06()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => !model.Bool), "!Bool()");
    }

    // Length
    [Test]
    public void LengthTest01()
    {
      RunTest((Expression<Func<TestModel, int>>)(model => model.A.Length), "A().length");
    }

    [Test]
    public void LengthTest02()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => model.A.Length > 0), "(A().length > 0)");
    }

    [Test]
    public void LengthTest03()
    {
      RunTest((Expression<Func<TestModel, int>>)(model => model.List.Count), "List().length");
    }

    // Nested
    [Test]
    public void NestedTest01()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.SubModel.A), "SubModel().A");
    }

    [Test]
    public void NestedTest02()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.SubModel.SubModel.SubModel.A), "SubModel().SubModel().SubModel().A");
    }

    // InstaceNames
    [Test]
    public void InstanceNamesTest01()
    {
      var data = new KnockoutExpressionData { InstanceNames = new[] { "$parent" } };
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "$parent.A", data);
    }

    [Test]
    public void InstanceNamesTest02()
    {
      var data = new KnockoutExpressionData { InstanceNames = new[] { "X", "Y", "Z" } };
      RunTest((Expression<Func<TestModel, TestModel, TestModel, string>>)((x, y, z) => x.A + y.B + z.C), "((X.A()+Y.B())+Z.C())", data);
    }

    // Aliases 
    [Test]
    public void AliasesTest01()
    {
      var model = new TestModel();
      var property = typeof(TestModel).GetProperty("Concatenation");
      var method = property.GetGetMethod();
      var expression = method.Invoke(model, null) as Expression;
      var data = KnockoutExpressionData.CreateConstructorData();
      data.Aliases[typeof(TestModel).FullName] = "this";
      RunTest(expression, "((('#'+this.A())+this.B())+this.C())", data);
    }

    //Contexts
    [Test]
    public void ContextTest01()
    {
      var viewContext = new ViewContext {Writer = new StringWriter()};
      var context = new KnockoutContext<TestModel>(viewContext);
      using (var subContext = context.With(m => m.SubModel))
      {
        using (var subSubContext = subContext.Foreach(m => m.List))
        {
          string bind = subSubContext.Bind.Text(m => context.Model.A + subContext.Model.B + m.ToString()).BindingAttributeContent();
          AssertStringEquivalent("text:(($parents[1].A()+$parent.B())+$data)", bind);
        }
      }
    }
  }
}

