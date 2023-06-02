using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class BindingState
    {
        private static HashSet<Type> pEnums = new HashSet<Type>();
        private static HashSet<Type> pClasses = new HashSet<Type>();
        private static HashSet<Type> pPrimitives = new HashSet<Type>();

        public static void AddEnum(Type type)
        {
            if(!pEnums.Contains(type))
                pEnums.Add(type);
        }
        public static void AddClass(Type type)
        {
            if (!pClasses.Contains(type))
                pClasses.Add(type);
        }
        public static void AddPrimitives(Type type)
        {
            if (!pPrimitives.Contains(type))
                pPrimitives.Add(type);
        }

        public static IList<Type> GetEnums()
        {
            return pEnums.ToList();
        }
        public static IList<Type> GetClasses()
        {
            return pClasses.ToList();
        }
        public static IList<Type> GetPrimitives()
        {
            return pPrimitives.ToList();
        }
    }
}
