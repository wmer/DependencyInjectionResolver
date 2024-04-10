using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Cache;
using DependencyInjectionResolver.Attributes;
using HelpersLibs.Reflection.Helpers;

namespace DependencyInjectionResolver.Helpers; 
internal class ClassResolverHelper {
    private readonly TypeHelper _typeHelper;
    private readonly ObjectCache _objectCache;
    private readonly ClassHelper _classHelper;
    private readonly ClassDependencyHelper _classDependencyHelper;
    private readonly ConstructorHelper _constructorHelper;

    public ClassResolverHelper(TypeHelper typeHelper, ClassDependencyHelper classDependencyHelper, ObjectCache objectCache) {
        _typeHelper = typeHelper;
        _objectCache = objectCache;
        _classHelper = new ClassHelper(_typeHelper);
        _classDependencyHelper = classDependencyHelper; 
        _constructorHelper = new ConstructorHelper();
        _objectCache.AddObjectInCache(typeof(ConstructorHelper), _constructorHelper);
    }
    
    public object Resolve(Type type) {
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
    
    private object[] ResolveDependencies(Type type, ParameterInfo[] paramters) {
        var objects = new object[paramters.Length];

        for (int i = 0; i < paramters.Length; i++) {
            ParameterInfo? param = paramters[i];
            if (_classDependencyHelper.ExistDependencyDefinedWithParamName(type, param.Name)) {
                objects[i] = _classDependencyHelper.TryGetDependency(type, param.Name);
            } else if (_classDependencyHelper.ExistDependencyDefinedWithPositionOfParameter(type, param.Position)) {
                objects[i] = _classDependencyHelper.TryGetDependency(type, param.Position);
            } else {
                var paramType = param.ParameterType;
                objects[i] = Resolve(paramType);
            }
        }
        return objects;
    }

    public void ResolveFields(object obj) {
        var fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++) {
            FieldInfo? field = fields[i];
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
        for (int i = 0; i < properties.Length; i++) {
            PropertyInfo? prop = properties[i];
            var attributes = prop.GetCustomAttributes(true);
            var inject = attributes.Where(x => x is InjectAttribute).FirstOrDefault() is InjectAttribute;
            if (inject) {
                var value = Resolve(prop.PropertyType);
                prop.SetValue(obj, value, null);
            }
        }
    }
}
