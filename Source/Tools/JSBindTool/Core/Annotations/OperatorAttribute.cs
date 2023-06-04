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
        Add = 1 << 0,
        Sub = 1 << 1,
        Mul = 1 << 2,
        Div = 1 << 3,
        Equal = 1 << 4
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
