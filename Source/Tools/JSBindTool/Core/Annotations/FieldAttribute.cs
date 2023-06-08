using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : Attribute
    {
        public string JSName { get; private set; }
        public string NativeName { get; private set; }
        public FieldAttribute(string jsName, string nativeName)
        {
            JSName = jsName;
            NativeName = nativeName;
        }
    }
}
