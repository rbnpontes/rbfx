using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class RefPtr<T> : TemplateObject
    {
        public RefPtr() : base(typeof(T), TemplateType.RefPtr)
        {
            if (typeof(T).BaseType != typeof(PrimitiveObject))
                throw new InvalidOperationException("RefPtr<T> type must be used with a object that inherits PrimitiveObject.");
        }
    }
}
