using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace PerpetuumSoft.Knockout.Tests
{
  [TestClass]
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
    [TestMethod]
    public void ConstructorCommonTest1()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "this.A()", KnockoutExpressionData.CreateConstructorData());
    }

    [TestMethod]
    public void ConstructorCommonTest2()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B), "(this.A() + this.B())", KnockoutExpressionData.CreateConstructorData());
    }

    [TestMethod]
    public void ConstructorCommonTest3()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B + "!"), "((this.A() + this.B()) + '!')", KnockoutExpressionData.CreateConstructorData());
    }

    // Common
    [TestMethod]
    public void CommonTest01()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "A");
    }

    [TestMethod]
    public void CommonTest02()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.A + model.B), "(A() + B())");
    }

    [TestMethod]
    public void CommonTest03()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.Bool ? model.A : "Line"), "Bool() ? A() : 'Line'");
    }

    [TestMethod]
    public void CommonTest04()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => true), "true");
    }

    [TestMethod]
    public void CommonTest05()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => false), "false");
    }

    [TestMethod]
    public void CommonTest06()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => !model.Bool), "!Bool()");
    }

    // Length
    [TestMethod]
    public void LengthTest01()
    {
      RunTest((Expression<Func<TestModel, int>>)(model => model.A.Length), "A().length");
    }

    [TestMethod]
    public void LengthTest02()
    {
      RunTest((Expression<Func<TestModel, bool>>)(model => model.A.Length > 0), "(A().length > 0)");
    }

    [TestMethod]
    public void LengthTest03()
    {
      RunTest((Expression<Func<TestModel, int>>)(model => model.List.Count), "List().length");
    }

    // Nested
    [TestMethod]
    public void NestedTest01()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.SubModel.A), "SubModel().A");
    }

    [TestMethod]
    public void NestedTest02()
    {
      RunTest((Expression<Func<TestModel, string>>)(model => model.SubModel.SubModel.SubModel.A), "SubModel().SubModel().SubModel().A");
    }

    // InstaceNames
    [TestMethod]
    public void InstanceNamesTest01()
    {
      var data = new KnockoutExpressionData { InstanceNames = new[] { "$parent" } };
      RunTest((Expression<Func<TestModel, string>>)(model => model.A), "$parent.A", data);
    }

    [TestMethod]
    public void InstanceNamesTest02()
    {
      var data = new KnockoutExpressionData { InstanceNames = new[] { "X", "Y", "Z" } };
      RunTest((Expression<Func<TestModel, TestModel, TestModel, string>>)((x, y, z) => x.A + y.B + z.C), "((X.A()+Y.B())+Z.C())", data);
    }

    // Aliases 
    [TestMethod]
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
    [TestMethod]
    public void ContextTest01()
    {
      var viewContext = new ViewContext { Writer = new StringWriter() };
      var context = new KnockoutContext<TestModel>(viewContext);
      using (var subContext = context.With(m => m.SubModel))
      {
        string bind = subContext.Bind.Text(m => context.Model.A + m.ToString()).BindingAttributeContent();
        AssertStringEquivalent("text:($parent.A()+$data)", bind);

        using (var subSubContext = subContext.Foreach(m => m.List))
        {
          string subBind = subSubContext.Bind.Text(m => context.Model.A + subContext.Model.B + m.ToString()).BindingAttributeContent();
          AssertStringEquivalent("text:(($parents[1].A()+$parent.B())+$data)", subBind);
        }
      }
    }

    [TestMethod]
    public void ContextTest02()
    {
      var viewContext = new ViewContext { Writer = new StringWriter() };
      var context = new KnockoutContext<TestModel>(viewContext);
      using (var subContext = context.Foreach(m => m.IntList))
      {
        string bind = subContext.Bind.Text(m => m).BindingAttributeContent();
        AssertStringEquivalent("text: $data", bind);
      }
    }

    [TestMethod]
    public void ContextTest03()
    {
      var viewContext = new ViewContext { Writer = new StringWriter() };
      var context = new KnockoutContext<TestModel>(viewContext);
      using (var subContext = context.Foreach(m => m.IntList))
      {
        string bind = subContext.Bind.Text(n => n + 1).BindingAttributeContent();
        AssertStringEquivalent("text:(parseInt($data)+parseInt(1))", bind);
      }
    }
  }
}
