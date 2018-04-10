using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers {
    internal class ClassHelper {
        private readonly TypeHelper _typeHelper;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();

        public ClassHelper(TypeHelper typeHelper) {
            _typeHelper = typeHelper;
        }

        public ParameterInfo[] GetParameters(Type type) {
            lock (_lock1) {
                ParameterInfo[] paramters = null;
                ConstructorInfo constructor = null;
                constructor = _typeHelper.IsDefinedSignature(type) ? 
                    type.GetConstructor(_typeHelper.GetDefinedSignature(type)) : GetConstructor(type);
                paramters = constructor.GetParameters();
                return paramters;
            }
        }

        private ConstructorInfo GetConstructor(Type type) {
            lock (_lock2) {
                var ctors = type.GetConstructors();
                var ctor = ctors.FirstOrDefault();
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
