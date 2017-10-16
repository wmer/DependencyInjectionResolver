using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal partial class InstanceHelper {
        private static Dictionary<Type, object> _objCache = new Dictionary<Type, object>();

        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();

        public bool ExistInstance(Type type) {
            lock (lock1) {
                if (type.GetTypeInfo().IsInterface) {
                    type = _typeHelper.TryGetImplementation(type);
                    if (type != null) {
                        return _objCache.ContainsKey(type);
                    }
                } else {
                    return _objCache.ContainsKey(type);
                }
                return false;
            }
        }
        
        public void AddObjectInCache(Type type, object obj) {
            lock (lock2) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                _objCache[type] = obj;
            }
        }
        
        public object TryGetInCache(Type type) {
            lock (lock3) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                object objeto = null;
                if (!_instanceOptions.ContainsKey(type) || _instanceOptions.ContainsKey(type) && _instanceOptions[type] == InstanceOptions.OneInstance) {
                    if (ExistInstance(type)) {
                        objeto = _objCache[type];
                    }
                }
                return objeto;
            }
        }
    }
}
