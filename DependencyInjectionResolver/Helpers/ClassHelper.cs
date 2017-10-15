using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal class ClassHelper {
        private TypeHelper _typeHelper;

        public ClassHelper(TypeHelper typeHelper) {
            _typeHelper = typeHelper;
        }

        public ParameterInfo[] GetParameters(Type type) {
            lock (new object()) {
                ParameterInfo[] paramters = null;
                ConstructorInfo constructor = null;
                if (_typeHelper.IsDefinedSignature(type)) {
                    constructor = type.GetConstructor(_typeHelper.GetDefinedSignature(type));
                } else {
                    constructor = GetConstructor(type);
                }
                paramters = constructor.GetParameters();
                return paramters;
            }
        }

        private ConstructorInfo GetConstructor(Type type) {
            lock (new object()) {
                var ctors = type.GetConstructors();
                var ctor = ctors[0];
                for (var i = 0; i < ctors.Count(); i++) {
                    if (ctors[i].GetParameters().Count() > ctor.GetParameters().Count()) {
                        ctor = ctors[i];
                    }
                }
                return ctor;
            }
        }
    }
}
