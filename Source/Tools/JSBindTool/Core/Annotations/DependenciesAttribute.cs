using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DependenciesAttribute : Attribute
    {
        public Type[] Types { get; private set; }
        public DependenciesAttribute(Type[] types)
        {
            Types = types;
        }
    }
}
