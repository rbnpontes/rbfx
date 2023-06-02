using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyNameAttribute : Attribute
    {
        public string Name { get; set; }
        public PropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
