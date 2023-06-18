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

        public static string GetTypeName(Type type)
        {
            return type.GetCustomAttribute<TypeNameAttribute>()?.Name ?? type.Name;
        }

        public static PropertyMapAttribute GetPropertyMap(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<PropertyMapAttribute>();
            if (attr is null)
                throw new NullReferenceException($"Property {prop.Name} does not contains PropertyMapAttribute");
            return attr;
        }
        public static string GetJSPropertyName(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<PropertyNameAttribute>();
            string propName = attr?.Name ?? prop.Name;
            propName = propName.Replace("Get", string.Empty).Replace("Set", string.Empty);
            return propName;
        }
        public static string GetGetterName(PropertyInfo prop)
        {
            return GetPropertyMap(prop).GetterName ?? prop.Name;
        }
        public static string GetSetterName(PropertyInfo prop)
        {
            return GetPropertyMap(prop).SetterName ?? prop.Name;
        }

        public static string GetMethodNativeName(MethodInfo method)
        {
            return method.GetCustomAttribute<MethodAttribute>()?.NativeName ?? method.Name;
        }
        public static string GetMethodName(MethodInfo method)
        {
            return CodeUtils.ToCamelCase(method.Name);
        }

        public static bool IsValidMethod(MethodInfo method)
        {
            return method.GetCustomAttribute<MethodAttribute>() != null;
        }
        public static bool IsValidProperty(PropertyInfo prop)
        {
            return !IsIgnored(prop) && prop.GetCustomAttribute<PropertyMapAttribute>() != null;
        }

        public static FieldAttribute GetFieldAttribute(FieldInfo field)
        {
            FieldAttribute? attr = field.GetCustomAttribute<FieldAttribute>();
            if (attr is null)
                throw new NullReferenceException("field does not contains FieldAttribute");
            return attr;
        }
    }
}
