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
                .Where(x => x.IsSubclassOf(typeof(ClassObject)) || x.IsSubclassOf(typeof(PrimitiveObject)))
                .Where(x => !AnnotationUtils.IsIgnored(x))
                .ToList()
                .ForEach(type =>
                {
                    if (type.IsSubclassOf(typeof(ClassObject)))
                    {
                        BindingState.AddClass(type);
                        // Collect all used Enums
                        type.GetMethods().ToList().ForEach(methodInfo =>
                        {
                            methodInfo.GetParameters().Where(x => x.ParameterType.IsEnum).ToList().ForEach(x => BindingState.AddEnum(x.ParameterType));
                            if (methodInfo.ReturnType.IsEnum)
                                BindingState.AddEnum(methodInfo.ReturnType);
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
                    else if(type.IsSubclassOf(typeof(PrimitiveObject)))
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
                header.Add("#include \"enum_bindings.h\"");
                header.Add("#include \"primitive_bindings.h\"");
                header.Add("#include \"class_bindings.h\"");
                header.AddNewLine(2);
                header.Namespace(Constants.Namespace, scope =>
                {
                    scope.Add($"void {Constants.MethodPrefix}_bindings_setup(duk_context* ctx);");
                });
                File.WriteAllText(Path.Join(outputPath, "Bindings.h"), header.ToString());
            }
            // Generate bindings setup source
            {
                CodeBuilder source = new CodeBuilder();
                source.IndentationSize = 0;
                HeaderUtils.EmitNotice(source);
                source.Add("#include \"Bindings.h\"");
                source.AddNewLine();
                source.Namespace(Constants.Namespace, scope =>
                {
                    scope.Add($"void {Constants.MethodPrefix}_bindings_setup(duk_context* ctx)");
                    scope.Scope(setupScope =>
                    {
                        setupScope.Add(
                            $"{Constants.MethodPrefix}_enum_bindings_setup(ctx);",
                            $"{Constants.MethodPrefix}_primitive_bindings_setup(ctx);",
                            $"{Constants.MethodPrefix}_class_bindings_setup(ctx);"
                        );
                    });
                });
                File.WriteAllText(Path.Join(outputPath, "Bindings.cpp"), source.ToString());
            }

            // Generate Files
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Enumerators");
            {
                var enums = BindingState.GetEnums().ToList();
                GenerateSetupHeader(outputPath, "enum_bindings", Constants.EnumIncludeSuffix, enums);
                GenerateSetupSource(outputPath, "enum_bindings", enums);
            }

            WorkerUtils.ForEach(BindingState.GetEnums(), type => GenerateBindingFiles<EnumGen>(type, outputPath, Constants.EnumIncludeSuffix));

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Primitives");
            {
                var primitives = BindingState.GetPrimitives().ToList();
                GenerateSetupHeader(outputPath, "primitive_bindings", Constants.PrimitiveIncludeSuffix, primitives);
                GenerateSetupSource(outputPath, "primitive_bindings", primitives);
            }
            WorkerUtils.ForEach(BindingState.GetPrimitives(), type => GenerateBindingFiles<PrimitiveGen>(type, outputPath, Constants.PrimitiveIncludeSuffix));

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-- Generating Classes");
            {
                var classes = BindingState.GetClasses().ToList();
                GenerateSetupHeader(outputPath, "class_bindings", Constants.ClassIncludeSuffix, classes);
                GenerateSetupSource(outputPath, "class_bindings", classes);
            }
            WorkerUtils.ForEach(BindingState.GetClasses(), type => GenerateBindingFiles<ClassGen>(type, outputPath, Constants.ClassIncludeSuffix));
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("--- Bindings has been Generated with Success ---");
        }

        private static void GenerateBindingFiles<TCodeGen>(Type type, string outputPath, string headerSuffix)
        {
            Console.WriteLine($"- {type.Name}");
            CodeGen? builder = Activator.CreateInstance(typeof(TCodeGen), type) as CodeGen;
            if (builder is null)
                throw new NullReferenceException("could not possible to create code gen type");
            builder.HeaderName = type.Name + headerSuffix;

            File.WriteAllText(Path.Join(outputPath, builder.HeaderName + ".h"), builder.BuildHeader().ToString());
            File.WriteAllText(Path.Join(outputPath, builder.HeaderName + ".cpp"), builder.BuildSource().ToString());
        }
        private static void GenerateSetupHeader(string outputPath, string prefix, string headerSuffix, List<Type> types)
        {
            CodeBuilder header = new CodeBuilder();
            header.IndentationSize = 0;
            header.Add($"void {Constants.MethodPrefix}_{prefix}_setup(duk_context* ctx);");

            CodeBuilder builder = new CodeBuilder();
            builder.IndentationSize = 0;
            HeaderUtils.EmitNotice(builder);

            types.ForEach(type => builder.Add($"#include \"{type.Name}{headerSuffix}.h\""));
            builder.Namespace(Constants.Namespace, scope => scope.Add(header));

            File.WriteAllText(Path.Join(outputPath, prefix+".h"), builder.ToString());
        }
        private static void GenerateSetupSource(string outputPath, string prefix, List<Type> types)
        {
            CodeBuilder builder = new CodeBuilder();
            builder.IndentationSize = 0;
            HeaderUtils.EmitNotice(builder);

            builder.Add($"#include \"{prefix}.h\"").AddNewLine();
            builder.Namespace(Constants.Namespace, source =>
            {
                source.Add($"void {Constants.MethodPrefix}_{prefix}_setup(duk_context* ctx)");
                source.Scope(scope =>
                {
                    types.ForEach(type =>
                    {
                        scope.Add($"{CodeUtils.GetMethodPrefix(type)}_setup(ctx);");
                    });
                });
            });

            File.WriteAllText(Path.Join(outputPath, prefix+".cpp"), builder.ToString());
        }
    }
}
