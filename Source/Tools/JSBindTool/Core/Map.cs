using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;

namespace JSBindTool.Core
{
    public abstract class Map : IHashable, ICodeEmitter
    {
        public Type KeyType { get; private set; }
        public Type ValueType { get; private set; }

        public Map(Type typeKey, Type valueType)
        {
            KeyType = typeKey;
            ValueType = valueType;
        }

        public static Map Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(Map)))
                throw new ArgumentException("Invalid inherit Map type.");
            Map? map = Activator.CreateInstance(type) as Map;
            if (map is null)
                throw new NullReferenceException("Created instance returns null");
            return map;
        }
        public static bool IsMapType(Type type)
        {
            return type.IsSubclassOf(typeof(Map));
        }
        public virtual uint ToHash()
        {
            return HashUtils.Hash("VariantMap");
        }

        public virtual void EmitWrite(string accessor, CodeBuilder code)
        {
            code
                .Add("duk_push_object(ctx);")
                .Add($"for (auto pair : {accessor})")
                .Scope(code =>
                {
                    CodeUtils.EmitValueWrite(KeyType, "pair.first", code);
                    CodeUtils.EmitValueWrite(ValueType, "pair.second", code);
                    code.Add("duk_put_prop(ctx, -3);");
                });
        }

        public virtual void EmitRead(string varName, string accessor, CodeBuilder code)
        {
            code
                .Add($"{GetNativeDeclaration()} {varName};")
                .Add($"duk_enum(ctx, {accessor}, 0);")
                .Add($"while (duk_next(ctx, -1, 1))")
                .Scope(code =>
                {
                    CodeUtils.EmitValueRead(ValueType, varName + "_value", "-1", code);
                    CodeUtils.EmitValueRead(KeyType, varName + "_key", "-2", code);

                    code
                        .Add($"{varName}.insert(ea::make_pair({varName}_key, {varName}_value));")
                        .Add("duk_pop_2(ctx);");
                });
        }

        public virtual string GetNativeDeclaration()
        {
            return $"ea::unordered_map<{CodeUtils.GetNativeDeclaration(KeyType)}, {CodeUtils.GetNativeDeclaration(ValueType)}>";
        }
    }
    public class StringMap<T> : Map
    {
        public StringMap() : base(typeof(string), typeof(T)) { } 
    }
    public class NumberMap<T> : Map
    {
        public NumberMap() : base(typeof(int), typeof(T)) { }
    }
    public class VariantMap : Map
    {
        public VariantMap() : base(typeof(StringHash), typeof(Variant)) { }
    }
    public class StringVariantMap : Map
    {
        public StringVariantMap() : base(typeof(string), typeof(Variant)) { }

        public override string GetNativeDeclaration()
        {
            return $"StringVariantMap";
        }

    }
}
