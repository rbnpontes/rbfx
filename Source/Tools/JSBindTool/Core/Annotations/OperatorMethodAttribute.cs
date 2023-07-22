using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OperatorMethodAttribute : Attribute
    {
        public OperatorType OperatorType { get; private set; }
        public OperatorMethodAttribute(OperatorType operatorType)
        {
            OperatorType = operatorType;
        }
    }
}
