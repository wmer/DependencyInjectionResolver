﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DependencyInjectionResolver.Helpers {
    internal class ClassDependencyHelper {
        private readonly Dictionary<(Type classType, String paramName), object> _parameterName;
        private readonly Dictionary<(Type classType, int paramPosition), object> _parameterPosition;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();
        private readonly object _lock5 = new object();
        private readonly object _lock6 = new object();

        public ClassDependencyHelper() {
            _parameterPosition = new Dictionary<(Type classType, int paramPosition), object>();
            _parameterName = new Dictionary<(Type classType, String paramName), object>();
        }

        public bool ExistDependencyDefinedWithParamName(Type type, String paramName) {
            lock (_lock1) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                return _parameterName.ContainsKey((type, paramName));
            }
        }

        public bool ExistDependencyDefinedWithPositionOfParameter(Type type, int paramPosition) {
            lock (_lock2) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                return _parameterPosition.ContainsKey((type, paramPosition));
            }
        }

        public void DefineDependency(Type type, String paramName, object obj) {
            lock (_lock3) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                _parameterName[(type, paramName)] = obj;
            }
        }

        public object TryGetDependency(Type type, String paramName) {
            lock (_lock4) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                if (ExistDependencyDefinedWithParamName(type, paramName)) {
                    return _parameterName[(type, paramName)];
                }
                return null;
            }
        }

        public void DefineDependency(Type type, int paramPosition, object obj) {
            lock (_lock5) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                _parameterPosition[(type, paramPosition)] = obj;
            }
        }

        public object TryGetDependency(Type type, int paramPosition) {
            lock (_lock6) {
                if (type.GetTypeInfo().IsInterface) {
                    throw new ArgumentException("type não pode ser uma interface.");
                }
                if (ExistDependencyDefinedWithPositionOfParameter(type, paramPosition)) {
                    return _parameterPosition[(type, paramPosition)];
                }
                return null;
            }
        }
    }
}
