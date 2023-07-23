using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomCodeAttribute : Attribute
    {
        public Type ReturnType { get; private set; }
        public Type[] ParameterTypes { get; private set; }

        public CustomCodeAttribute()
        {
            ReturnType = typeof(void);
            ParameterTypes = new Type[0];
        }
        public CustomCodeAttribute(Type returnType)
        {
            ReturnType = returnType;
            ParameterTypes = new Type[0];
        }
        public CustomCodeAttribute(Type returnType, Type[] args)
        {
            ReturnType = returnType;
            ParameterTypes = args;
        }
        public CustomCodeAttribute(Type[] args)
        {
            ReturnType = typeof(void);
            ParameterTypes = args;
        }
    }
}
