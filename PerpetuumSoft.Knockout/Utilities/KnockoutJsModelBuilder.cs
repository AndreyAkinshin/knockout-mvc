using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DelegateDecompiler;

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
            if (modelType.IsClass && modelType.Namespace.Equals("System.Data.Entity.DynamicProxies"))
                modelType = modelType.BaseType;
            foreach (var property in modelType.GetProperties())
                if (property.GetCustomAttributes(typeof(ComputedAttribute), false).Any())
                {
                    sb.Append(modelName);
                    sb.Append('.');
                    sb.Append(property.Name);
                    sb.Append(" = ");
                    sb.Append("ko.computed(function() { try { return ");
                    sb.Append(ExpressionToString(modelType, DecompileExpressionVisitor.Decompile(Expression.Property(Expression.Constant(model), property))));
                    sb.Append(string.Format("}} catch(e) {{ return null; }}  ;}}, {0});", modelName));
                    sb.AppendLine();
                }

            foreach (var property in modelType.GetProperties())
            {
                if (property.PropertyType.IsClass && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    if (property.GetCustomAttributes(typeof(Newtonsoft.Json.JsonIgnoreAttribute), false).Length > 0)
                        continue;
                    try
                    {
                        var value = property.GetValue(model, null);
                        sb.Append(AddComputedToModel(property.PropertyType, value, modelName + "." + property.Name));
                    }
                    catch { }
                }
            }

            return sb.ToString();
        }

        public static string CreateMappingData<TModel>()
        {
            Type modelType = typeof(TModel);
            var builder = new StringBuilder();
            builder.AppendLine("{");

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
                            builder.Append(',');
                        builder.Append("'");
                        builder.Append(property.Name);
                        builder.AppendLine("': { create: function(options) {");
                        builder.AppendLine("var data = ko.mapping.fromJS(options.data);");

                        builder.Append(computed);

                        builder.AppendLine("return data;");
                        builder.AppendLine("}}");
                    }
                }
            }

            builder.Append("}");

            if (first)
                return "{}";

            return builder.ToString();
        }

        private static string ExpressionToString(Type modelType, Expression expression)
        {
            var data = KnockoutExpressionData.CreateConstructorData();
            data.Aliases[modelType.FullName] = "this";
            return KnockoutExpressionConverter.Convert(expression, data);
        }
    }
}
