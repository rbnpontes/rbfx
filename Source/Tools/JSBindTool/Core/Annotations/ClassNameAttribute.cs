using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ClassNameAttribute : Attribute
    {
        public string ClassName { get; private set; }
        public ClassNameAttribute(string className)
        {
            ClassName = className;
        }
    }
}
