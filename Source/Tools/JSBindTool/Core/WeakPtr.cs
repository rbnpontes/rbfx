using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class WeakPtr<T> : TemplateObject
    {
        public WeakPtr() : base(typeof(T), TemplateType.WeakPtr) { }
    }
}
