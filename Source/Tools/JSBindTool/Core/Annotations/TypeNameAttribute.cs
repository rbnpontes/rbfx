using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TypeNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public string JSName { get; private set; }

        public TypeNameAttribute(string name)
        {
            Name = JSName = name;
        }
        public TypeNameAttribute(string name, string jsName)
        {
            Name = name;
            JSName = jsName;
        }
    }
}
