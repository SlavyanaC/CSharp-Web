using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.Framework.Services.Contracts;

namespace SIS.Framework.Services
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> dependencyDictionary;

        public DependencyContainer()
        {
            this.dependencyDictionary = new Dictionary<Type, Type>();
        }

        private Type this[Type key] =>
            this.dependencyDictionary.ContainsKey(key) ? this.dependencyDictionary[key] : null;

        public void RegisterDependency<TSource, TDestination>()
        {
            this.dependencyDictionary[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)this.CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            var instanceType = this[type] ?? type;

            if (instanceType.IsInterface || instanceType.IsAbstract)
            {
                throw new InvalidOperationException($"Type {instanceType.FullName} cannot be instantiated.");
            }

            var constructor = instanceType.GetConstructors().OrderBy(c => c.GetParameters().Length).First();
            var constructorParameters = constructor.GetParameters();
            var constructorParameterObjects = new object[constructorParameters.Length];

            for (var i = 0; i < constructorParameterObjects.Length; i++)
            {
                constructorParameterObjects[i] = this.CreateInstance(constructorParameters[i].ParameterType);
            }

            return constructor.Invoke(constructorParameterObjects);
        }
    }
}
