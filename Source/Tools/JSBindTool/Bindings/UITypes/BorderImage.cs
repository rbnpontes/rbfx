using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Cursor.h")]
    [Abstract]
    public class BorderImage : UIElement
    {
        public BorderImage() : base(typeof(BorderImage)) { }
        public BorderImage(Type type) : base(type) { }

        [Method]
        [CustomCode(new Type[]
        {
            typeof(Array),
            typeof(Array),
            typeof(IntRect)
        })]
        public void GetBatches(CodeBuilder code)
        {
            code
                .Add("ea::vector<UIBatch> batches;")
                .Add("ea::vector<float> vertexData;")
                .Add("instance->GetBatches(batches, vertexData, arg2);")
                .Add("for (unsigned i =0; i < batches.size(); ++i)")
                .Scope(code =>
                {
                });
        }
    }
}
