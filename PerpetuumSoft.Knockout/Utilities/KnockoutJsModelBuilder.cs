using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PerpetuumSoft.Knockout
{
  public static class KnockoutJsModelBuilder
  {
    public static string AddComputedToModel<TModel>(TModel model, string modelName)
    {
      return AddComputedToModel(typeof(TModel), model, modelName);
    }

    public static string AddComputedToModel(Type modelType, object model, string modelName)
    {
      var sb = new StringBuilder();

      foreach (var method in modelType.GetMethods())
      {
        if (typeof(Expression).IsAssignableFrom(method.ReturnType))
        {
          sb.Append(modelName);
          sb.Append('.');
          sb.Append(method.Name);
          sb.Append(" = ");
          sb.Append("ko.computed(function() { try { return ");
          sb.Append(GetGetExpression(modelType, model, method));
          sb.Append(string.Format("}} catch(e) {{ return null; }}  ;}}, {0});", modelName));
          sb.AppendLine();
        }
      }

      return sb.ToString();
    }

    public static string CreateMappingData<TModel>()
    {
      Type modelType = typeof(TModel);
      var sb = new StringBuilder();
      sb.AppendLine("{");

      bool first = true;
      foreach (var property in modelType.GetProperties())
      {
        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) &&
            !typeof(string).IsAssignableFrom(property.PropertyType) &&
            property.PropertyType.IsGenericType)
        {
          Type itemType = property.PropertyType.GetGenericArguments()[0];
          var constructor = itemType.GetConstructor(new Type[0]);
          if (constructor != null)
          {
            var item = constructor.Invoke(null);
            var computed = AddComputedToModel(itemType, item, "data");
            if (string.IsNullOrWhiteSpace(computed))
              continue;
            if (first)
              first = false;
            else
              sb.Append(',');            
            sb.Append("'");
            sb.Append(property.Name);
            sb.AppendLine("': { create: function(options) {");
            sb.AppendLine("var data = ko.mapping.fromJS(options.data);");

            sb.Append(computed);

            sb.AppendLine("return data;");
            sb.AppendLine("}}");
          }
        }
      }      

      sb.Append("}");

      if (first)
        return "{}";

      return sb.ToString();
    }

    private static string GetGetExpression(Type modelType, object model, MethodInfo method)
    {      
      var expression = method.Invoke(model, null) as Expression;
      var data = KnockoutExpressionData.CreateConstructorData();
      data.Aliases[modelType.FullName] = "this";
      return KnockoutExpressionConverter.Convert(expression, data);
    }
  }
}
