using Reflection.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Cache;
using DependencyInjectionResolver.Attributes;

namespace DependencyInjectionResolver.Helpers {
    internal class ClassResolverHelper {
        private readonly TypeHelper _typeHelper;
        private readonly ObjectCache _objectCache;
        private readonly ClassHelper _classHelper;
        private readonly ClassDependencyHelper _classDependencyHelper;
        private readonly ConstructorHelper _constructorHelper;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();

        public ClassResolverHelper(TypeHelper typeHelper, ClassDependencyHelper classDependencyHelper, ObjectCache objectCache) {
            _typeHelper = typeHelper;
            _objectCache = objectCache;
            _classHelper = new ClassHelper(_typeHelper);
            _classDependencyHelper = classDependencyHelper; 
            _constructorHelper = new ConstructorHelper();
            _objectCache.AddObjectInCache(typeof(ConstructorHelper), _constructorHelper);
        }
        
        public object Resolve(Type type) {
            lock (_lock1) {
                if (type.GetTypeInfo().IsInterface) {
                    type = _typeHelper.GetImplementation(type);
                }
                object obj = _objectCache.TryGetInCache(type);
                if (obj != null) return obj;
                ParameterInfo[] paramters = _classHelper.GetParameters(type);
                if (!paramters.Any()) {
                    obj = _constructorHelper.CreateConstructor(type, Type.EmptyTypes)();
                    _objectCache.AddObjectInCache(type, obj);
                } else {
                    var objects = ResolveDependencies(type, paramters);
                    var pameters = _typeHelper.ParamaterInfoToType(paramters).ToArray();
                    obj = _constructorHelper.CreateConstructor(type, pameters)(objects);
                    _objectCache.AddObjectInCache(type, obj);
                }

                ResolveFields(obj);
                ResolveProperties(obj);

                return obj;
            }
        }
        
        private object[] ResolveDependencies(Type type, ParameterInfo[] paramters) {
            lock (_lock2) {
                var objects = new object[paramters.Length];
                var i = 0;
                foreach (var param in paramters) {
                    if (_classDependencyHelper.ExistDependencyDefinedWithParamName(type, param.Name)) {
                        objects[i] = _classDependencyHelper.TryGetDependency(type, param.Name);
                    } else if (_classDependencyHelper.ExistDependencyDefinedWithPositionOfParameter(type, param.Position)) {
                        objects[i] = _classDependencyHelper.TryGetDependency(type, param.Position);
                    } else {
                        var paramType = param.ParameterType;
                        objects[i] = Resolve(paramType);
                    }
                    i++;
                }
                return objects;
            }
        }

        public void ResolveFields(object obj) {
            var fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields) {
                var attributes = field.GetCustomAttributes(true);
                var inject = attributes.Where(x => x is InjectAttribute).FirstOrDefault() is InjectAttribute;
                if (inject) {
                    var value = Resolve(field.FieldType);
                    field.SetValue(obj, value);
                }
            }
        }

        public void ResolveProperties(object obj) {
            var properties = obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties) {
                var attributes = prop.GetCustomAttributes(true);
                var inject = attributes.Where(x => x is InjectAttribute).FirstOrDefault() is InjectAttribute;
                if (inject) {
                    var value = Resolve(prop.PropertyType);
                    prop.SetValue(obj, value, null);
                }
            }
        }
    }
}
