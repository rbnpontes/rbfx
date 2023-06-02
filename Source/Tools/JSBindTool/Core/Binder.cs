using JSBindTool.Bindings;
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
                .Where(x => x.IsSubclassOf(typeof(EngineObject)))
                .ToList()
                .ForEach(type =>
                {
                    BindingState.AddClass(type);
                    // Collect all used Enums
                    type.GetMethods(BindingFlags.Public).ToList().ForEach(methodInfo =>
                    {
                        methodInfo.GetGenericArguments().Where(x => x.IsEnum).ToList().ForEach(x => BindingState.AddEnum(x));
                        if (methodInfo.ReturnParameter.ParameterType.IsEnum)
                            BindingState.AddEnum(methodInfo.ReturnParameter.ParameterType);
                    });
                    type.GetProperties(BindingFlags.Public).ToList().ForEach(propInfo =>
                    {
                        if (propInfo.PropertyType.IsEnum)
                            BindingState.AddEnum(propInfo.PropertyType);
                    });
                    type.GetFields(BindingFlags.Public).ToList().ForEach(fieldInfo =>
                    {
                        if (fieldInfo.FieldType.IsEnum)
                            BindingState.AddEnum(fieldInfo.FieldType);
                    });
                });
        }
        public static void GenerateBindings(string outputPath)
        {
            if (!File.Exists(outputPath))
                throw new ArgumentException("Provided output path does not exists.", "outputPath");
            // Generate Directories
            Directory.CreateDirectory(Path.Join(outputPath, "include"));
            Directory.CreateDirectory(Path.Join(outputPath, "src"));

            // Generate all headers into one file
            StringBuilder mainHeaders = new StringBuilder();
            mainHeaders.AppendLine("// # generated file by the JSBindTool\n");
            mainHeaders.AppendLine("// Enum headers");
            BindingState.GetEnums().ToList().ForEach(type => mainHeaders.AppendLine($"#include \"{type.Name}_Enum.h\""));
            mainHeaders.AppendLine("// Primitive headers");
            BindingState.GetPrimitives().ToList().ForEach(type => mainHeaders.AppendLine($"#include \"{type.Name}.h\""));
            mainHeaders.AppendLine("// Class headers");
            BindingState.GetClasses().ToList().ForEach(type => mainHeaders.AppendLine($"#include \"{type.Name}.h\""));

            File.WriteAllText(Path.Join(outputPath, "include", "Headers.h"), mainHeaders.ToString());

            // Generate Files
            Console.WriteLine("Generating Enumerators");

            Parallel.ForEach(BindingState.GetEnums(), typeEnum =>
            {
                Console.WriteLine($"- {typeEnum.Name}.h");
            });
            Parallel.ForEach(BindingState.GetPrimitives(), primitiveEnum =>
            {

            });
            Parallel.ForEach(BindingState.GetClasses(), typeClass =>
            {

            });
        }
    }
}
