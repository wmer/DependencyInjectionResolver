using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver.Helpers;

namespace DependencyInjectionResolver.Cache; 
internal class ObjectCache(TypeHelper typeHelper) {
    private readonly TypeHelper _typeHelper = typeHelper;
    private static readonly Dictionary<Type, object> ObjCache = [];

    public bool ExistInstance(Type type) {
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

    public void AddObjectInCache(Type type, object obj) {
        if (type.GetTypeInfo().IsInterface) {
            throw new ArgumentException("type não pode ser uma interface.");
        }
        ObjCache[type] = obj;
    }

    public object TryGetInCache(Type type) {
        if (type.GetTypeInfo().IsInterface) {
            throw new ArgumentException("type não pode ser uma interface.");
        }
        object objeto = null;
        if (ExistInstance(type)) {
            objeto = ObjCache[type];
        }
        return objeto;
    }

    public void DeleteInCache(Type type) {
        if (type.GetTypeInfo().IsInterface) {
            throw new ArgumentException("type não pode ser uma interface.");
        }
        if (ExistInstance(type)) {
            ObjCache.Remove(type);
        }
    }
}
