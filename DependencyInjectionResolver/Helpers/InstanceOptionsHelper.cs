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
        
        public void DefineInstanceOptions(Type type, InstanceOptions option, bool overrideoption = true) {
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

        private bool ExistOverrideOptions(Type type) {
            return _overrideOptions.ContainsKey(type);
        }
    }
}
