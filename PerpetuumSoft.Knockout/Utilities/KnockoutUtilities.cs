using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PerpetuumSoft.Knockout
{
  public static class KnockoutUtilities
  {
    private static readonly List<AssemblyName> systemNames = new List<AssemblyName>
        {
        new AssemblyName ("mscorlib, Version=4.0.0.0, Culture=neutral, " +
                          "PublicKeyToken=b77a5c561934e089"),
        new AssemblyName ("System.Core, Version=4.0.0.0, Culture=neutral, "+
                          "PublicKeyToken=b77a5c561934e089")
        };

    public static void ConvertData(object data)
    {
      if (data == null)
        return;
      foreach (var property in data.GetType().GetProperties())
      {
        if (property.GetGetMethod() == null)
          continue;
        if (property.GetGetMethod().GetParameters().Length > 0)
          continue;
        var value = property.GetValue(data, null);
        if (value == null)
        {
          value = GetActualValue(property.PropertyType, null);
          if (value != null)
            property.SetValue(data, GetActualValue(property.PropertyType, null), null);
        }
        else if (!IsSystemType(property.PropertyType))
          ConvertData(value);
      }
    }

    public static object GetActualValue(Type type, object value)
    {
      if (value == null)
      {
        if (typeof(string).IsAssignableFrom(type))
          return "";
        if (typeof(IList).IsAssignableFrom(type))
          return type.GetConstructor(new Type[0]).Invoke(null);
      }
      return value;
    }

    private static bool IsSystemType(Type type)
    {
      var objAN = new AssemblyName(type.Assembly.FullName);
      return systemNames.Any(n => n.Name == objAN.Name && 
                                  n.GetPublicKeyToken().SequenceEqual(objAN.GetPublicKeyToken()));
    }
  }
}