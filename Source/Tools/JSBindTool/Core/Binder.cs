using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class Binder
    {
        public static void CollectTypes()
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsSubclassOf(typeof(EngineObject)) || x.IsSubclassOf(typeof(PrimitiveObject)))
                .Where(x => !AnnotationUtils.IsIgnored(x))
                .ToList()
                .ForEach(type =>
                {
                    if (type.IsSubclassOf(typeof(EngineObject)))
                    {
                        BindingState.AddClass(type);
                        // Collect all used Enums
                        type.GetMethods().ToList().ForEach(methodInfo =>
                        {
                            methodInfo.GetGenericArguments().Where(x => x.IsEnum).ToList().ForEach(x => BindingState.AddEnum(x));
                            if (methodInfo.ReturnParameter.ParameterType.IsEnum)
                                BindingState.AddEnum(methodInfo.ReturnParameter.ParameterType);
                        });
                        type.GetProperties().ToList().ForEach(propInfo =>
                        {
                            if (propInfo.PropertyType.IsEnum)
                                BindingState.AddEnum(propInfo.PropertyType);
                        });
                        type.GetFields().ToList().ForEach(fieldInfo =>
                        {
                            if (fieldInfo.FieldType.IsEnum)
                                BindingState.AddEnum(fieldInfo.FieldType);
                        });
                    }
                    else
                    {
                        BindingState.AddPrimitives(type);
                    }
                });
        }
        public static void GenerateBindings(string outputPath)
        {
            if (!Directory.Exists(outputPath))
                throw new ArgumentException("Provided output path does not exists.", "outputPath");

            // Generate bindings setup header
            {
                CodeBuilder header = new CodeBuilder();
                header.IndentationSize = 0;
                HeaderUtils.EmitNotice(header);
                header.Add("#pragma once");
                header.Add("#include \"EnumBindings.h\"");
                header.Add("#include \"PrimitiveBindings.h\"");
                header.Add("#include \"ClassBindings.h\"");
                header.AddNewLine(2);
                header.Add("namespace Urho3D {");
                {
                    CodeBuilder setupSignature = new CodeBuilder();
                    setupSignature.Add("void Bindings_setup(duk_context* ctx);");
                    header.Add(setupSignature);
                }
                header.Add("}");
                File.WriteAllText(Path.Join(outputPath, "Bindings.h"), header.ToString());
            }
            // Generate bindings setup source
            {
                CodeBuilder source = new CodeBuilder();
                source.IndentationSize = 0;
                HeaderUtils.EmitNotice(source);
                source.Add("#include \"Bindings.h\"");
                source.AddNewLine();
                source.Add("namespace Urho3D");
                source.Add("{");
                {
                    CodeBuilder setupBody = new CodeBuilder();
                    setupBody.Add("void Bindings_setup(duk_context* ctx)");
                    setupBody.Add("{");
                    {
                        CodeBuilder scope = new CodeBuilder();
                        scope.Add(
                            "EnumBindings_setup(ctx);",
                            "PrimitiveBindings_setup(ctx);",
                            "ClassBindings_setup(ctx);"
                        );
                        setupBody.Add(scope);
                    }
                    setupBody.Add("}");
                    source.Add(setupBody);
                }
                source.Add("}");
                File.WriteAllText(Path.Join(outputPath, "Bindings.cpp"), source.ToString());
            }

            // Generate Files
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Enumerators");
            {
                var enums = BindingState.GetEnums().ToList();
                GenerateSetupHeader(outputPath, "EnumBindings", "Enum", enums);
                GenerateSetupSource(outputPath, "EnumBindings", "Enum", enums);
            }

            WorkerUtils.ForEach(BindingState.GetEnums(), type => GenerateBindingFiles<EnumGen>(type, outputPath, "_Enum"));

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Primitives");
            {
                var primitives = BindingState.GetPrimitives().ToList();
                GenerateSetupHeader(outputPath, "PrimitiveBindings", "Primitive", primitives);
                GenerateSetupSource(outputPath, "PrimitiveBindings", "Primitive", primitives);
            }
            WorkerUtils.ForEach(BindingState.GetPrimitives(), type => GenerateBindingFiles<PrimitiveGen>(type, outputPath, "_Primitive"));

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Classes");
            {
                var classes = BindingState.GetClasses().ToList();
                GenerateSetupHeader(outputPath, "ClassBindings", "Class", classes);
                GenerateSetupSource(outputPath, "ClassBindings", "Class", classes);
            }
            WorkerUtils.ForEach(BindingState.GetClasses(), type => GenerateBindingFiles<ClassGen>(type, outputPath, "_Class"));
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("--- Bindings has been Generated with Success ---");
        }

        private static void GenerateBindingFiles<TCodeGen>(Type type, string outputPath, string headerSuffix)
        {
            Console.WriteLine($"- {type.Name}");
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            CodeGen builder = (CodeGen)Activator.CreateInstance(typeof(TCodeGen), type);
            builder.HeaderName = type.Name + headerSuffix;

            File.WriteAllText(Path.Join(outputPath, builder.HeaderName + ".h"), builder.BuildHeader().ToString());
            File.WriteAllText(Path.Join(outputPath, builder.HeaderName + ".cpp"), builder.BuildSource().ToString());
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }
        private static void GenerateSetupHeader(string outputPath, string prefix, string headerSuffix, List<Type> types)
        {
            CodeBuilder header = new CodeBuilder();
            header.Add($"void {prefix}_setup(duk_context* ctx);");

            CodeBuilder builder = new CodeBuilder();
            builder.IndentationSize = 0;
            HeaderUtils.EmitNotice(builder);

            types.ForEach(type => builder.Add($"#include \"{type.Name}_{headerSuffix}.h\""));
            builder.Namespace("Urho3D", scope => scope.Add(header));

            File.WriteAllText(Path.Join(outputPath, prefix+".h"), builder.ToString());
        }
        private static void GenerateSetupSource(string outputPath, string prefix, string headerSuffix, List<Type> types)
        {
            CodeBuilder builder = new CodeBuilder();
            builder.IndentationSize = 0;
            HeaderUtils.EmitNotice(builder);

            builder.Add($"#include \"{prefix}.h\"");
            builder.Namespace("Urho3D", source =>
            {
                source.Add($"void {prefix}_setup(duk_context* ctx)");
                source.Scope(scope =>
                {
                    types.ForEach(type =>
                    {
                        scope.Add($"{type.Name}_setup(ctx);");
                    });
                });
            });

            File.WriteAllText(Path.Join(outputPath, prefix+".cpp"), builder.ToString());
        }
    }
}
