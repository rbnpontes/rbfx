using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public abstract class CodeGen
    {
        public Type Target { get; private set; }
        public string HeaderName { get; set; } = string.Empty;
        public CodeGen(Type type)
        {
            Target = type;
        }

        protected virtual CodeBuilder GetIncludes()
        {
            CodeBuilder codeBuilder = new CodeBuilder();
            codeBuilder.IndentationSize = 0;
            codeBuilder.Add(GetIncludesFromType().ToList().Select(include => $"#include <{include}>"));
            codeBuilder.Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");
            codeBuilder.Add("#include <Urho3D/JavaScript/JavaScriptSystem.h>");
            codeBuilder.Add("#include <duktape/duktape.h>");
            return codeBuilder;
        }
        protected HashSet<string> GetIncludesFromType()
        {
            HashSet<string> includes = new HashSet<string>();
            var attributes = Target.GetCustomAttributes<IncludeAttribute>();
            attributes.ToList().ForEach(attr =>
            {
                if (!includes.Contains(attr.Value))
                    includes.Add(attr.Value);
            });

            return includes;
        }
        protected string ToCamelCase(string input)
        {
            if (input.Length == 0)
                return string.Empty;
            char[] chars = input.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }

        public abstract CodeBuilder BuildHeader();
        public abstract CodeBuilder BuildSource();
    }
}
