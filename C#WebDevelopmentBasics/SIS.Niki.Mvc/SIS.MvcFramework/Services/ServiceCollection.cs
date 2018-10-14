namespace SIS.MvcFramework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class ServiceCollection : IServiceCollection
    {
        private readonly IDictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)this.CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            if (this.dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }

            if (type.IsInterface || type.IsAbstract)
            {
                throw new Exception($"Type {type.FullName} cannot be instantiated.");
            }

            //TODO: if empty -> use it
            var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).First();
            var constructorParams = constructor.GetParameters();
            var constructorParamObjects = new List<object>();

            foreach (var constructorParam in constructorParams)
            {
                var parameterObject = this.CreateInstance(constructorParam.ParameterType);
                constructorParamObjects.Add(parameterObject);
            }

            var obj = constructor.Invoke(constructorParamObjects.ToArray());
            return obj;
        }
    }
}
