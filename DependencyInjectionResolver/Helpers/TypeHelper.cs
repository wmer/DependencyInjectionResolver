using HelpersLibs.Reflection.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers; 
internal class TypeHelper {
    private static readonly Dictionary<Type, Type> DefinitionCache = [];
    private static readonly Dictionary<Type, Type> _interfaceDefinition = [];
    private readonly Dictionary<Type, Type[]> _signature;

    public TypeHelper() {
        _signature = [];
    }

    public bool IsDefined(Type type) => _interfaceDefinition.ContainsKey(type);


    public void DefineImplementation(Type interfaceType, Type implementationType) => _interfaceDefinition[interfaceType] = implementationType;

    public bool IsDefinedSignature(Type classType) {
        if (classType.GetTypeInfo().IsInterface) {
            throw new ArgumentException("classType não pode ser uma interface.");
        }
        return _signature.ContainsKey(classType);
    }

    public void DefineSignature(Type classType, params Type[] signature) {
        if (classType.GetTypeInfo().IsInterface) {
            throw new ArgumentException("classType não pode ser uma interface.");
        }
        _signature[classType] = signature;
    }

    public Type[] GetDefinedSignature(Type classType) {
        if (classType.GetTypeInfo().IsInterface) {
            throw new ArgumentException("classType não pode ser uma interface.");
        }
        return _signature[classType];
    }

    public Type GetDefinedImplementation(Type interfaceType) => _interfaceDefinition[interfaceType];

    public void AddInCache(Type interfaceType, Type implementationType) => DefinitionCache[interfaceType] = implementationType;

    public Type TryGetImplementation(Type interfaceType) {
        if (IsDefined(interfaceType)) {
            return GetDefinedImplementation(interfaceType);
        }
        return TryGetFromCache(interfaceType);
    }

    public Type? TryGetFromCache(Type interfaceType) {
        if (DefinitionCache.TryGetValue(interfaceType, out Type? value)) {
            return value;
        }
        return null;
    }

    public IEnumerable<Type> ParamaterInfoToType(ParameterInfo[] parameters) {
        for (int i = 0; i < parameters.Length; i++) {
            ParameterInfo? parameter = parameters[i];
            yield return parameter.ParameterType;
        }
    }

    public Type GetImplementation(Type type) {
        var typegeneric = type;
        Type implementationType = null;
        Type[] argumentsTypes = null;
        var isGeneric = type.GetTypeInfo().IsGenericType;
        if (TryGetImplementation(type) != null) {
            implementationType = TryGetImplementation(type);
        } else {
            if (isGeneric) {
                argumentsTypes = type.GetGenericArguments();
                type = type.GetGenericTypeDefinition();
            }
            implementationType = AssemblyHelper.GetTypes(x => x.GetTypeInfo().GetInterface(type.FullName) != null, type.GetTypeInfo().Assembly, true).First();
            if (isGeneric) {
                implementationType = implementationType.MakeGenericType(argumentsTypes);
                AddInCache(typegeneric, implementationType);
            } else {
                AddInCache(type, implementationType);
            }
        }
        return implementationType;
    }
}
