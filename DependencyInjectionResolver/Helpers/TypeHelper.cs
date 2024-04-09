using Reflection.Optimization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace DependencyInjectionResolver.Helpers {
    internal class TypeHelper {
        private static readonly Dictionary<Type, Type> DefinitionCache = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> _interfaceDefinition = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type[]> _signature;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();
        private readonly object _lock5 = new object();
        private readonly object _lock6 = new object();
        private readonly object _lock7 = new object();
        private readonly object _lock8 = new object();
        private readonly object _lock9 = new object();
        private readonly object _lock10 = new object();
        private readonly object _lock11 = new object();

        public TypeHelper() {
            _signature = new Dictionary<Type, Type[]>();
        }

        public bool IsDefined(Type type) {
            lock (_lock1) {
                return _interfaceDefinition.ContainsKey(type);
            }
        }

        public void DefineImplementation(Type interfaceType, Type implementationType) {
            lock (_lock2) {
                _interfaceDefinition[interfaceType] = implementationType;
            }
        }

        public bool IsDefinedSignature(Type classType) {
            lock (_lock3) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                return _signature.ContainsKey(classType);
            }
        }

        public void DefineSignature(Type classType, params Type[] signature) {
            lock (_lock4) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                _signature[classType] = signature;
            }
        }

        public Type[] GetDefinedSignature(Type classType) {
            lock (_lock5) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                return _signature[classType];
            }
        }

        public Type GetDefinedImplementation(Type interfaceType) {
            lock (_lock6) {
                return _interfaceDefinition[interfaceType];
            }
        }

        public void AddInCache(Type interfaceType, Type implementationType) {
            lock (_lock7) {
                DefinitionCache[interfaceType] = implementationType;
            }
        }

        public Type TryGetImplementation(Type interfaceType) {
            lock (_lock8) {
                if (IsDefined(interfaceType)) {
                    return GetDefinedImplementation(interfaceType);
                }
                return TryGetFromCache(interfaceType);
            }
        }

        public Type TryGetFromCache(Type interfaceType) {
            lock (_lock9) {
                if (DefinitionCache.ContainsKey(interfaceType)) {
                    return DefinitionCache[interfaceType];
                }
                return null;
            }
        }

        public IEnumerable<Type> ParamaterInfoToType(ParameterInfo[] parameters) {
            lock (_lock10) {
                foreach (var parameter in parameters) {
                    yield return parameter.ParameterType;
                }
            }
        }

        public Type GetImplementation(Type type) {
            lock (_lock11) {
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
    }
}
