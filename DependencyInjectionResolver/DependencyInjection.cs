using DependencyInjectionResolver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Cache;

namespace DependencyInjectionResolver {
    public partial class DependencyInjection {
        private readonly TypeHelper _typeHelper;
        private readonly ObjectCache _objectCache;
        private readonly ClassDependencyHelper _classDependencyHelper;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();
        private readonly object _lock5 = new object();
        private readonly object _lock6 = new object();

        public DependencyInjection() {
            _typeHelper = new TypeHelper();
            _classDependencyHelper = new ClassDependencyHelper();
            _objectCache = new ObjectCache(_typeHelper);
        }

        public DependencyInjection BindingTypes(Type interfaceType, Type implementationType) {
            lock (_lock1) {
                _typeHelper.DefineImplementation(interfaceType, implementationType);
                return this;
            }
        }

        public DependencyInjection DefineConstructorSignature(Type implementationType, params Type[] paramsTypes) {
            lock (_lock2) {
                _typeHelper.DefineSignature(implementationType, paramsTypes);
                return this;
            }
        }

        public DependencyInjection DefineDependency(Type classType, String parameterName, object parameterValue) {
            lock (_lock3) {
                _classDependencyHelper.DefineDependency(classType, parameterName, parameterValue);
                return this;
            }
        }

        public DependencyInjection DefineDependency(Type classType, int parameterPosition, object parameterValue) {
            lock (_lock4) {
                _classDependencyHelper.DefineDependency(classType, parameterPosition, parameterValue);
                return this;
            }
        }
        
        public DependencyInjection DefineLifeTimeOptions(Type type, InstanceOptions lifetumeOptions) {
            lock (_lock5) {
                if (lifetumeOptions == InstanceOptions.DiferentInstances) {
                    _objectCache.DeleteInCache(type);
                }
                return this;
            }
        }
        
        public object Resolve(Type type, InstanceOptions lifetumeOptions = InstanceOptions.OneInstance) {
            lock (_lock6) {
                DefineLifeTimeOptions(type, lifetumeOptions);
                return new ClassResolverHelper(_typeHelper, _classDependencyHelper, _objectCache).Resolve(type);
            }
        }
    }
}