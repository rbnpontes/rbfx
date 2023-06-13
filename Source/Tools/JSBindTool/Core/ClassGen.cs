using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class ClassGen : CodeGen
    {
        public ClassGen(Type type) : base(type)
        {
        }

        public override CodeBuilder BuildHeader()
        {
            ClassObject obj = ClassObject.Create(Target);

            CodeBuilder header = new CodeBuilder();
            header.IndentationSize = 0;
            HeaderUtils.EmitNotice(header);
            header.Add("#pragma once");

            obj.EmitHeaderIncludes(header);

            header.Namespace("Urho3D", (signatures) =>
            {
                obj.EmitHeaderSignatures(signatures);
            });

            return header;
        }

        public override CodeBuilder BuildSource()
        {
            ClassObject obj = ClassObject.Create(Target);

            CodeBuilder source = new CodeBuilder();
            source.IndentationSize = 0;
            HeaderUtils.EmitNotice(source);
            obj.EmitSourceIncludes(source);

            source.Namespace("Urho3D", (sourceScope) =>
            {
                obj.EmitSource(sourceScope);
            });

            return source;
        }

        protected override CodeBuilder GetIncludes()
        {
            CodeBuilder includes = base.GetIncludes();
            if(Target.BaseType != null)
            {
                if (Target.BaseType == typeof(ClassObject))
                    includes.Add("#include <Urho3D/Core/Object.h>");
                else
                    includes.Add($"#include \"{Target.BaseType.Name}_Class.h\"");
            }

            return includes;
        }
    }
}
