using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [Flags]
    public enum OperatorFlags
    {
        None = 0,
        Add,
        Sub,
        Mul,
        Div,
        Equal
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OperatorAttribute : Attribute
    {
        public OperatorFlags Flags { get; private set; }
        public OperatorAttribute(OperatorFlags flags)
        {
            Flags = flags;
        }
    }
}
