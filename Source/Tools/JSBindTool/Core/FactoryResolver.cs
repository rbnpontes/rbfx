using System.Reflection;

namespace JSBindTool.Core.Annotations
{
    public interface IFactoryResolver<T>
    {
        T Build(Type type);
    }


    class BaseResolver<T>
    {
        public Dictionary<Type, T> pResolvers = new Dictionary<Type, T>();

        protected T BuildResolver(Type type)
        {
            T? instance;
            if (pResolvers.TryGetValue(type, out instance))
                return instance;
            object? resolver = Activator.CreateInstance(type);
            if (resolver is null)
                throw new NullReferenceException("could not possible to create resolver type.");
            T result = (T)resolver;
            pResolvers.Add(type, result);
            return result;
        }
    }
    class FactoryPropertyResolver : BaseResolver<IPropertyResolver>, IFactoryResolver<IPropertyResolver>
    {
        public IPropertyResolver Build(Type type)
        {
            return BuildResolver(type);
        }
    }

    public static class FactoryResolver
    {
        private static FactoryPropertyResolver pFactoryPropResolver = new FactoryPropertyResolver();

        public static IPropertyResolver Build(PropertyInfo prop)
        {
            PropertyResolverAttribute attr = AnnotationUtils.GetPropertyResolver(prop);
            return pFactoryPropResolver.Build(attr.ResolverType);
        }
    }
}
