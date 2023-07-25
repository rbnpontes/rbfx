using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NamespaceAttribute : Attribute
    {
        public string Namespace { get; private set; }
        public NamespaceAttribute(string value)
        {
            Namespace = value;
        }
    }
}
