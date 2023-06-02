using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyMapAttribute : Attribute
    {
        public string? GetterName { get; set; }
        public string? SetterName { get; set; }
        public PropertyMapAttribute(string? getterName, string? setterName = null)
        {
            GetterName = getterName;
            SetterName = setterName;
        }
    }
}
