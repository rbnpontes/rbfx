using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class WorkerUtils
    {
        public static void ForEach<T>(IEnumerable<T> source, Action<T> body)
        {
#if DEBUG
            foreach(var item in source)
                body(item);
#else
            Parallel.ForEach(source, body);
#endif
        }
    }
}
