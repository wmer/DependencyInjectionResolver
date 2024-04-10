using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Helpers; 
internal class ClassHelper(TypeHelper typeHelper) {
    private readonly TypeHelper _typeHelper = typeHelper;

    public ParameterInfo[] GetParameters(Type type) {
        ParameterInfo[] paramters = null;
        ConstructorInfo constructor = null;
        constructor = _typeHelper.IsDefinedSignature(type) ?
            type.GetConstructor(_typeHelper.GetDefinedSignature(type)) : GetConstructor(type);
        paramters = constructor.GetParameters();
        return paramters;
    }

    private ConstructorInfo GetConstructor(Type type) {
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
