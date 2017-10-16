using Reflection.Optimization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal class TypeHelper {
        private static Dictionary<Type, Type> definitionCache = new Dictionary<Type, Type>();
        private Dictionary<Type, Type> _interfaceDefinition;
        private Dictionary<Type, Type[]> _signature;

        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock4 = new object();
        private object lock5 = new object();
        private object lock6 = new object();
        private object lock7 = new object();
        private object lock8 = new object();
        private object lock9 = new object();
        private object lock10 = new object();
        private object lock11 = new object();

        public TypeHelper() {
            _interfaceDefinition = new Dictionary<Type, Type>();
            _signature = new Dictionary<Type, Type[]>();
        }

        public bool IsDefined(Type type) {
            lock (lock1) {
                return _interfaceDefinition.ContainsKey(type);
            }
        }

        public void DefineImplementation(Type interfaceType, Type implementationType) {
            lock (lock2) {
                _interfaceDefinition[interfaceType] = implementationType;
            }
        }

        public bool IsDefinedSignature(Type classType) {
            lock (lock3) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                return _signature.ContainsKey(classType);
            }
        }

        public void DefineSignature(Type classType, params Type[] signature) {
            lock (lock4) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                _signature[classType] = signature;
            }
        }

        public Type[] GetDefinedSignature(Type classType) {
            lock (lock5) {
                if (classType.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("classType não pode ser uma interface.");
                }
                return _signature[classType];
            }
        }

        public Type GetDefinedImplementation(Type interfaceType) {
            lock (lock6) {
                return _interfaceDefinition[interfaceType];
            }
        }

        public void AddInCache(Type interfaceType, Type implementationType) {
            lock (lock7) {
                definitionCache[interfaceType] = implementationType;
            }
        }

        public Type TryGetImplementation(Type interfaceType) {
            lock (lock8) {
                if (IsDefined(interfaceType)) {
                    return GetDefinedImplementation(interfaceType);
                }
                return TryGetFromCache(interfaceType);
            }
        }

        public Type TryGetFromCache(Type interfaceType) {
            lock (lock9) {
                if (definitionCache.ContainsKey(interfaceType)) {
                    return definitionCache[interfaceType];
                }
                return null;
            }
        }

        public IEnumerable<Type> ParamaterInfoToType(ParameterInfo[] parameters) {
            lock (lock10) {
                foreach (var parameter in parameters) {
                    yield return parameter.ParameterType;
                }
            }
        }

        public Type GetImplementation(Type type) {
            lock (lock11) {
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
