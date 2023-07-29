using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class DebugUtils
    {
        public static void Break()
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
