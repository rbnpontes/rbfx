using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.GraphicsTypes
{
    public interface IDFamily
    {
        uint ObjectID { get; }
        void AcquireObjectID();
        void ReleaseObjectID();
    }
}
