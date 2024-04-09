using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionResolver {
    public partial class DependencyInjection {
        public DependencyInjection BindingTypes<T, U>() =>
            BindingTypes(typeof(T), typeof(U));

        public DependencyInjection DefineConstructorSignature<T>(params Type[] paramsTypes) =>
            DefineConstructorSignature(typeof(T), paramsTypes);

        public DependencyInjection DefineDependency<T>(String parameterName, object parameterValue) =>
            DefineDependency(typeof(T), parameterName, parameterValue);

        public DependencyInjection DefineDependency<T>(int parameterPosition, object parameterValue) =>
            DefineDependency(typeof(T), parameterPosition, parameterValue);

        public DependencyInjection DefineLifeTimeOptions<T>(InstanceOptions lifetumeOptions) =>
            DefineLifeTimeOptions(typeof(T), lifetumeOptions);

        public T Resolve<T>(InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) =>
            (T)Resolve(typeof(T), lifetumeOptions);
    }
}
