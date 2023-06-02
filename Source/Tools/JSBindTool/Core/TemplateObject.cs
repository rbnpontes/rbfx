using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class TemplateObject<T>
    {
        public Type TargetType { get; private set; }
        public T Target { get; private set; }
        public TemplateObject()
        {
            TargetType = typeof(T);
            Target = Activator.CreateInstance<T>();
        }
    }
}
