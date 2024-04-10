using DependencyInjectionResolver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Cache;

namespace DependencyInjectionResolver; 
public partial class DependencyInjection {
    private readonly TypeHelper _typeHelper;
    private readonly ObjectCache _objectCache;
    private readonly ClassDependencyHelper _classDependencyHelper;

    public DependencyInjection() {
        _typeHelper = new TypeHelper();
        _classDependencyHelper = new ClassDependencyHelper();
        _objectCache = new ObjectCache(_typeHelper);
    }

    public DependencyInjection BindingTypes(Type interfaceType, Type implementationType) {
        _typeHelper.DefineImplementation(interfaceType, implementationType);
        return this;
    }

    public DependencyInjection DefineConstructorSignature(Type implementationType, params Type[] paramsTypes) {
        _typeHelper.DefineSignature(implementationType, paramsTypes);
        return this;
    }

    public DependencyInjection DefineDependency(Type classType, String parameterName, object parameterValue) {
        _classDependencyHelper.DefineDependency(classType, parameterName, parameterValue);
        return this;
    }

    public DependencyInjection DefineDependency(Type classType, int parameterPosition, object parameterValue) {
        _classDependencyHelper.DefineDependency(classType, parameterPosition, parameterValue);
        return this;
    }
    
    public DependencyInjection DefineLifeTimeOptions(Type type, InstanceOptions lifetumeOptions) {
        if (lifetumeOptions == InstanceOptions.DiferentInstances) {
            _objectCache.DeleteInCache(type);
        }
        return this;
    }
    
    public object Resolve(Type type, InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
        DefineLifeTimeOptions(type, lifetumeOptions);
        return new ClassResolverHelper(_typeHelper, _classDependencyHelper, _objectCache).Resolve(type);
    }
}