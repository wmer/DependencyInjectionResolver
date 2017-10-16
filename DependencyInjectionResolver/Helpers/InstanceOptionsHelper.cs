using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal partial class InstanceHelper {
        private Dictionary<Type, InstanceOptions> _instanceOptions;
        private Dictionary<Type, bool> _overrideOptions;

        private object lock12 = new object();
        private object lock13 = new object();
        
        public void DefineInstanceOptions(Type type, InstanceOptions option, bool overrideoption = true) {
            lock (lock12) {
                if (type.GetTypeInfo().IsInterface) {
                    type = _typeHelper.GetImplementation(type);
                }
                if (!overrideoption) {
                    _overrideOptions[type] = overrideoption;
                    _instanceOptions[type] = option;
                }
                if (!ExistOverrideOptions(type)) {
                    _instanceOptions[type] = option;
                }
            }
        }

        private bool ExistOverrideOptions(Type type) {
            lock (lock13) {
                return _overrideOptions.ContainsKey(type);
            }
        }
    }
}
