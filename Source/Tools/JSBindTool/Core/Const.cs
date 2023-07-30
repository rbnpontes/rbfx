using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class Const<T> : TemplateObject
    {
        public Const() : base(typeof(T), TemplateType.Const)
        {
        }
    }
}
