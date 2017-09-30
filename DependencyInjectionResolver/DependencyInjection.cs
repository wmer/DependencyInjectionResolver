using DependencyInjectionResolver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver {
    public class DependencyInjection {
        private TypeHelper _typeHelper;
        private InstanceHelper _instanceHelper;
        private ClassDependencyHelper _classDependencyHelper;

        public DependencyInjection() {
            _typeHelper = new TypeHelper();
            _classDependencyHelper = new ClassDependencyHelper();
            _instanceHelper = new InstanceHelper(_typeHelper);
        }

        public DependencyInjection BindingTypes(Type interfaceType, Type implementationType, InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
            _typeHelper.DefineImplementation(interfaceType, implementationType);
            _instanceHelper.DefineInstanceOptions(implementationType, lifetumeOptions);
            return this;
        }

        public DependencyInjection BindingTypes<U, V>(InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
            return BindingTypes(typeof(U), typeof(V), lifetumeOptions);
        }

        public DependencyInjection DefineConstructorSignature(Type implementationType, params Type[] paramsTypes) {
            _typeHelper.DefineSignature(implementationType, paramsTypes);
            return this;
        }

        public DependencyInjection DefineConstructorSignature<T>(params Type[] paramsTypes) {
            return DefineConstructorSignature(typeof(T), paramsTypes);
        }

        public DependencyInjection DefineDependency(Type classType, String parameterName, object parameterValue) {
            _classDependencyHelper.DefineDependency(classType, parameterName, parameterValue);
            return this;
        }

        public DependencyInjection DefineDependency<U>(String parameterName, object parameterValue) {
            DefineDependency(typeof(U), parameterName, parameterValue);
            return this;
        }

        public DependencyInjection DefineDependency(Type classType, int parameterPosition, object parameterValue) {
            _classDependencyHelper.DefineDependency(classType, parameterPosition, parameterValue);
            return this;
        }

        public DependencyInjection DefineDependency<U>(int parameterPosition, object parameterValue) {
            DefineDependency(typeof(U), parameterPosition, parameterValue);
            return this;
        }

        public DependencyInjection DefineLifeTimeOptions<U>(InstanceOptions lifetumeOptions) {
            _instanceHelper.DefineInstanceOptions(typeof(U), lifetumeOptions, false);
            return this;
        }
        
        public object Resolve(Type type, InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
            _instanceHelper.DefineInstanceOptions(type, lifetumeOptions);
            return new ClassResolverHelper(_typeHelper, _classDependencyHelper, _instanceHelper).Resolve(type);
        }
        
        public T Resolve<T>(InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
            return (T) Resolve(typeof(T), lifetumeOptions);
        }
    }
}