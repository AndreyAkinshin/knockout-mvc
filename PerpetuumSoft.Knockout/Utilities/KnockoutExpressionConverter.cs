using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace PerpetuumSoft.Knockout
{
  public class KnockoutExpressionConverter
  {
    private KnockoutExpressionData data;

    private readonly List<string> lambdaFrom = new List<string>();

    public static string Convert(Expression expression, KnockoutExpressionData convertData = null)
    {
      var converter = new KnockoutExpressionConverter();
      return converter.LocalConvert(expression, convertData);
    }

    private string LocalConvert(Expression expression, KnockoutExpressionData convertData = null)
    {
      lambdaFrom.Clear();
      data = convertData ?? new KnockoutExpressionData();
      data = data.Clone();
      if (data.InstanceNames == null || data.InstanceNames.Length == 0)
        data.InstanceNames = new[] { "" };
      return GetterSetterCorrecting(Visit(expression));
    }

    //TODO: rewrite
    public string GetterSetterCorrecting(string expr)
    {
      int cnt = expr.Count(c => char.IsLetterOrDigit(c) || c == '(' || c == ')' || c == '.' || c == '$');
      if (cnt == expr.Length && expr.EndsWith("()") && !data.NeedBracketsForAllMembers)
        expr = expr.Substring(0, expr.Length - 2);
      return expr;
    }

    protected virtual string Visit(Expression exp)
    {
      if (exp == null)
        throw new ArgumentNullException();
      switch (exp.NodeType)
      {
        case ExpressionType.Negate:
        case ExpressionType.NegateChecked:
          return VisitUnary((UnaryExpression)exp, "-");
        case ExpressionType.Not:
          return VisitUnary((UnaryExpression)exp, "!");
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
          return VisitUnary((UnaryExpression)exp, "");
        case ExpressionType.ArrayLength:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          throw new NotSupportedException();
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
          return VisitBinary((BinaryExpression)exp, "+");
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return VisitBinary((BinaryExpression)exp, "-");
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
          return VisitBinary((BinaryExpression)exp, "*");
        case ExpressionType.Divide:
          return VisitBinary((BinaryExpression)exp, "/");
        case ExpressionType.Modulo:
          return VisitBinary((BinaryExpression)exp, "%");
        case ExpressionType.And:
          return VisitBinary((BinaryExpression)exp, "&");
        case ExpressionType.AndAlso:
          return VisitBinary((BinaryExpression)exp, "&&");
        case ExpressionType.Or:
          return VisitBinary((BinaryExpression)exp, "|");
        case ExpressionType.OrElse:
          return VisitBinary((BinaryExpression)exp, "||");
        case ExpressionType.LessThan:
          return VisitBinary((BinaryExpression)exp, "<");
        case ExpressionType.LessThanOrEqual:
          return VisitBinary((BinaryExpression)exp, "<=");
        case ExpressionType.GreaterThan:
          return VisitBinary((BinaryExpression)exp, ">");
        case ExpressionType.GreaterThanOrEqual:
          return VisitBinary((BinaryExpression)exp, ">=");
        case ExpressionType.Equal:
          return VisitBinary((BinaryExpression)exp, "==");
        case ExpressionType.NotEqual:
          return VisitBinary((BinaryExpression)exp, "!=");
        case ExpressionType.Coalesce:
          throw new NotSupportedException();
        case ExpressionType.ArrayIndex:
          return VisitArrayIndex((BinaryExpression)exp);
        case ExpressionType.RightShift:
          return VisitBinary((BinaryExpression)exp, ">>");
        case ExpressionType.LeftShift:
          return VisitBinary((BinaryExpression)exp, "<<");
        case ExpressionType.ExclusiveOr:
          return VisitBinary((BinaryExpression)exp, "^");
        case ExpressionType.TypeIs:
          return VisitTypeIs((TypeBinaryExpression)exp);
        case ExpressionType.Conditional:
          return VisitConditional((ConditionalExpression)exp);
        case ExpressionType.Constant:
          return VisitConstant((ConstantExpression)exp);
        case ExpressionType.Parameter:
          return VisitParameter((ParameterExpression)exp);
        case ExpressionType.MemberAccess:
          return VisitMemberAccess((MemberExpression)exp);
        case ExpressionType.Call:
          return VisitMethodCall((MethodCallExpression)exp);
        case ExpressionType.Lambda:
          return VisitLambda((LambdaExpression)exp);
        case ExpressionType.New:
          return VisitNew((NewExpression)exp);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return VisitNewArray((NewArrayExpression)exp);
        case ExpressionType.Invoke:
          return VisitInvocation((InvocationExpression)exp);
        case ExpressionType.MemberInit:
          return VisitMemberInit((MemberInitExpression)exp);
        case ExpressionType.ListInit:
          return VisitListInit((ListInitExpression)exp);
        default:
          throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
      }
    }
    
    protected virtual string VisitMemberAccess(MemberExpression m)
    {      
      return VisitMemberAccess(m.Expression, m.Member.Name);
    }

    //TODO: rewrite
    private string VisitMemberAccess(Expression obj, string member)
    {      
      if (typeof(IKnockoutContext).IsAssignableFrom(obj.Type))
      {
        var lambda = Expression.Lambda<Func<IKnockoutContext>>(obj);
        var context = lambda.Compile()();
        if (member == "Model")
          return context.GetInstanceName();
      }
      var own = Visit(obj);
      if (data.Aliases.ContainsKey(own))
        own = data.Aliases[own];
      if (lambdaFrom.Contains(own))
        own = data.InstanceNames[lambdaFrom.IndexOf(own)];
      if ((member == "Length" || member == "Count") && !data.InstanceNames.Contains(own))
        member = "length";
      string prefix = own == "" ? "" : own + ".";
      string suffix = member == "length" ? "" : "()";
      string result = prefix + member + suffix;
      if (data.Aliases.ContainsKey(result))
        result = data.Aliases[result];
      else if (data.Aliases.ContainsKey(prefix + member))
        result = data.Aliases[prefix + member];
      return result;
    }

    protected virtual string VisitUnary(UnaryExpression u, string sign)
    {
      string operand = Visit(u.Operand);
      return sign + operand;
    }

    protected virtual string VisitBinary(BinaryExpression b, string sign)
    {
      if (b.NodeType == ExpressionType.Coalesce)
        throw new NotSupportedException();
      string left = Visit(b.Left);
      string right = Visit(b.Right);
      return left + sign + right;
    }

    private string VisitArrayIndex(BinaryExpression b)
    {
      string array = Visit(b.Left);
      string index = Visit(b.Right);
      return array + "[" + index + "]";
    }

    protected virtual string VisitConstant(ConstantExpression c)
    {
      if (c.Value is string && !((string)c.Value).StartsWith("$"))
        return "'" + c.Value + "'";
      if (c.Value is bool)
        return ((bool)c.Value) ? "true" : "false";
      return c.Value == null ? "null" : c.Value.ToString();
    }

    protected virtual string VisitConditional(ConditionalExpression c)
    {
      string test = Visit(c.Test);
      string ifTrue = Visit(c.IfTrue);
      string ifFalse = Visit(c.IfFalse);
      return test + " ? " + ifTrue + " : " + ifFalse;
    }

    protected virtual string VisitParameter(ParameterExpression p)
    {
      if (lambdaFrom.Contains(p.Name))
        return data.InstanceNames[lambdaFrom.IndexOf(p.Name)];
      return p.Name;
    }

    protected virtual string VisitLambda(LambdaExpression lambda)
    {
      foreach (var parameter in lambda.Parameters)
        lambdaFrom.Add(parameter.Name);
      return Visit(lambda.Body);
    }

    protected virtual string VisitTypeIs(TypeBinaryExpression b)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitMethodCall(MethodCallExpression m)
    {
      if (m.Arguments.Count == 1 && m.Method.Name == "get_Item")
      {
        string array = Visit(m.Object);
        string index = Visit(m.Arguments[0]);
        return array + "[" + index + "]";
      }
      if (m.Arguments.Count == 0 && m.Method.Name == "ToString")
      {
        if (m.Object is ParameterExpression && lambdaFrom.Contains((m.Object as ParameterExpression).Name))
          return "$data";
        string obj = Visit(m.Object);
        return obj;
      }
      if (m.Arguments.Count == 1 && m.Object == null && m.Method.Name == "ToBoolean")
      {
        string obj = Visit(m.Arguments[0]);
        return obj;
      }
      if (typeof(Expression).IsAssignableFrom(m.Method.ReturnType))
        return VisitMemberAccess(m.Object, m.Method.Name);
      throw new NotSupportedException();
    }

    protected virtual ReadOnlyCollection<string> VisitExpressionList(ReadOnlyCollection<Expression> original)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitMemberAssignment(MemberAssignment assignment)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitMemberListBinding(MemberListBinding binding)
    {
      throw new NotSupportedException();
    }

    protected virtual IEnumerable<string> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitNew(NewExpression nex)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitMemberInit(MemberInitExpression init)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitListInit(ListInitExpression init)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitNewArray(NewArrayExpression na)
    {
      throw new NotSupportedException();
    }

    protected virtual string VisitInvocation(InvocationExpression iv)
    {
      throw new NotSupportedException();
    }
  }
}
