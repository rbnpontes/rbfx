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
        private static HashSet<Type> pModules = new HashSet<Type>();
        private static HashSet<Type> pStructs = new HashSet<Type>();

        public static void AddEnum(Type type)
        {
            pEnums.Add(type);
        }
        public static void AddClass(Type type)
        {
            pClasses.Add(type);
        }
        public static void AddPrimitive(Type type)
        {
            pPrimitives.Add(type);
        }
        public static void AddModule(Type type)
        {
            pModules.Add(type);
        }
        public static void AddStruct(Type type)
        {
            pStructs.Add(type);
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
        public static IList<Type> GetModules()
        {
            return pModules.ToList();
        }
        public static IList<Type> GetStructs()
        {
            return pStructs.ToList();
        }
    }
}
