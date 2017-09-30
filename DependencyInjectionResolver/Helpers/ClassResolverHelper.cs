using Reflection.Optimization;
using Reflection.Optimization.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal class ClassResolverHelper {
        private TypeHelper _typeHelper;
        private InstanceHelper _instanceHelper;
        private ClassHelper _classHelper;
        private ClassDependencyHelper _classDependencyHelper;
        private ReflectionOptimizations _reflectionOptimizations;

        public ClassResolverHelper(TypeHelper typeHelper, ClassDependencyHelper classDependencyHelper, InstanceHelper instanceHelper) {
            _typeHelper = typeHelper;
            _instanceHelper = instanceHelper;
            _classHelper = new ClassHelper(_typeHelper);
            _classDependencyHelper = classDependencyHelper;
            _reflectionOptimizations = new ReflectionOptimizations();
            _instanceHelper.AddObjectInCache(typeof(ReflectionOptimizations), _reflectionOptimizations);
        }
        
        public object Resolve(Type type) {
            if (type.GetTypeInfo().IsInterface) {
                type = _typeHelper.GetImplementation(type);
            }
            object obj = _instanceHelper.TryGetInCache(type);
            if (obj != null) return obj;
            ParameterInfo[] paramters = _classHelper.GetParameters(type);
            if (paramters.Count() == 0) {
                obj = _reflectionOptimizations.CreateConstructor(type, Type.EmptyTypes)();
                _instanceHelper.AddObjectInCache(type, obj);
            } else {
                var objects = ResolveDependencies(type, paramters);
                var pameters = _typeHelper.ParamaterInfoToType(paramters).ToArray();
                obj = _reflectionOptimizations.CreateConstructor(type, pameters)(objects);
                _instanceHelper.AddObjectInCache(type, obj);
            }
            return obj;
        }
        
        private object[] ResolveDependencies(Type type, ParameterInfo[] paramters) {
            var objects = new object[paramters.Length];
            var i = 0;
            foreach (var param in paramters) {
                if (_classDependencyHelper.ExistDependencyDefinedWithParamName(type, param.Name)) {
                    objects[i] = _classDependencyHelper.TryGtDependency(type, param.Name);
                } else if (_classDependencyHelper.ExistDependencyDefinedWithPositionOfParameter(type, param.Position)) {
                    objects[i] = _classDependencyHelper.TryGtDependency(type, param.Position);
                } else {
                    var paramType = param.ParameterType;
                    if (paramType.GetTypeInfo().IsInterface) {
                        paramType = _typeHelper.GetImplementation(paramType);
                    }
                    objects[i] = Resolve(paramType);
                }
                i++;
            }
            return objects;
        }
    }
}
