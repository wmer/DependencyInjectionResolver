using DependencyInjectionResolver.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Extensions {
    public static class DependencyInjectionExtension {
        public static T Diferent<T>(this T obj) {
            var dependencies = new DependencyInjection<T>();
            return dependencies
                        .BindingTypes(typeof(T), obj.GetType())
                        .DefineLifeTimeOptions<T>(InstanceOptions.DiferentInstances)
                        .Resolve();
        }

        public static DependencyInjection<T> DiferentWith<T>(this T obj) {
            return new DependencyInjection<T>()
                            .BindingTypes(typeof(T), obj.GetType())
                            .DefineLifeTimeOptions<T>(InstanceOptions.DiferentInstances);
        }
    }
}
