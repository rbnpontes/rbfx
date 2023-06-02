using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IncludeAttribute : Attribute
    {
        public IncludeAttribute(string includePath) { }
    }
}
