using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class VariableAttribute : Attribute
    {
        public string Value { get; private set; }
        public VariableAttribute(string name)
        {
            Value = name;
        }
    }
}
