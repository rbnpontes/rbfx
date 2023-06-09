using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MethodAttribute : Attribute
    {
        public string NativeName { get; private set; }
        public MethodAttribute(string name)
        {
            NativeName = name;
        }
    }
}
