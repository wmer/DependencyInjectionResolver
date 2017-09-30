using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal partial class InstanceHelper {
        private TypeHelper _typeHelper;

        public InstanceHelper(TypeHelper typeHelper) {
            _overrideOptions = new Dictionary<Type, bool>();
            _instanceOptions = new Dictionary<Type, InstanceOptions>();
            _typeHelper = typeHelper;
        }
    }
}
