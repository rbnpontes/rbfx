using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefineAttribute : Attribute
    {
        public string CustomCode { get; private set; }
        public DefineAttribute(string customCode)
        {
            CustomCode = customCode;
        }
    }
}
