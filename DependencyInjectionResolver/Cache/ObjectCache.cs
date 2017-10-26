using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Helpers;

namespace DependencyInjectionResolver.Cache {
    internal class ObjectCache {
        private readonly TypeHelper _typeHelper;
        private static readonly Dictionary<Type, object> ObjCache = new Dictionary<Type, object>();

        public ObjectCache(TypeHelper typeHelper) {
            _typeHelper = typeHelper;
        }

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();

        public bool ExistInstance(Type type) {
            lock (_lock1) {
                if (type.GetTypeInfo().IsInterface) {
                    type = _typeHelper.TryGetImplementation(type);
                    if (type != null) {
                        return ObjCache.ContainsKey(type);
                    }
                } else {
                    return ObjCache.ContainsKey(type);
                }
                return false;
            }
        }

        public void AddObjectInCache(Type type, object obj) {
            lock (_lock2) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                ObjCache[type] = obj;
            }
        }

        public object TryGetInCache(Type type) {
            lock (_lock3) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                object objeto = null;
                if (ExistInstance(type)) {
                    objeto = ObjCache[type];
                }
                return objeto;
            }
        }

        public void DeleteInCache(Type type) {
            lock (_lock4) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                if (ExistInstance(type)) {
                    ObjCache.Remove(type);
                }
            }
        }
    }
}
