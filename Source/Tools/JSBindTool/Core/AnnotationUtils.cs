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
    }
}
