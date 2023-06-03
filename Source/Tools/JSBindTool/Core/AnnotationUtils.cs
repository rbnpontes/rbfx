using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class AnnotationUtils
    {
        public static OperatorFlags GetOperatorFlags(Type type)
        {
            return type.GetCustomAttribute<OperatorAttribute>()?.Flags ?? 0;
        }
        public static bool IsIgnored(Type type)
        {
            return type.GetCustomAttribute<IgnoreAttribute>() != null;
        }
        public static bool IsIgnored(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<IgnoreAttribute>() != null;
        }
        public static bool IsIgnored(FieldInfo field)
        {
            return field.GetCustomAttribute<IgnoreAttribute>() != null;
        }
        public static bool IsIgnored(MethodInfo field)
        {
            return field.GetCustomAttribute<IgnoreAttribute>() != null;
        }
        public static bool IsAbstract(Type type)
        {
            return type.GetCustomAttribute<AbstractAttribute>() != null;
        }
        public static List<string> GetIncludes(Type type)
        {
            var includes = type.GetCustomAttributes<IncludeAttribute>();
            return includes.Select(x => x.Value).ToList();
        }
        public static string GetVariableName(PropertyInfo propInfo)
        {
            return propInfo.GetCustomAttribute<VariableAttribute>()?.Value ?? propInfo.Name;
        }
        public static string GetVariableName(FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<VariableAttribute>()?.Value ?? fieldInfo.Name;
        }

        public static string GetClassName(Type type)
        {
            return type.GetCustomAttribute<ClassNameAttribute>()?.ClassName ?? type.Name;
        }

        public static string GetPropertyName(PropertyInfo prop)
        {
            string propName = prop.GetCustomAttribute<PropertyMapAttribute>()?.GetterName ?? prop.Name;
            propName = propName.Replace("Get", string.Empty).Replace("Set", string.Empty);
            return propName;
        }
        public static string GetGetterName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<PropertyMapAttribute>()?.GetterName ?? prop.Name;
        }
        public static string GetSetterName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<PropertyMapAttribute>()?.SetterName ?? prop.Name;
        }

        public static bool IsValidMethod(MethodInfo method)
        {
            return method.GetCustomAttribute<MethodAttribute>() != null;
        }
        public static bool IsValidProperty(PropertyInfo prop)
        {
            return !IsIgnored(prop) && prop.GetCustomAttribute<PropertyMapAttribute>() != null;
        }
    }
}
