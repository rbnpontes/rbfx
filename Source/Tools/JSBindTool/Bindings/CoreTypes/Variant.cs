using JSBindTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.CoreTypes
{
    public class Variant : NoopPrimitive
    {
        public Variant() : base(typeof(Variant)) { }
    }
}
