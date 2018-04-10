using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionResolver.Extensions {
    public static class DependencyInjectionExtension {
        public static T Diferent<T>(this T obj) {
            var dependencies = new DependencyInjection();
            return dependencies
                        .BindingTypes(typeof(T), obj.GetType())
                        .DefineLifeTimeOptions<T>(InstanceOptions.DiferentInstances)
                        .Resolve<T>();
        }

        public static DependencyInjection DiferentWith<T>(this T obj) {
            return new DependencyInjection()
                            .BindingTypes(typeof(T), obj.GetType())
                            .DefineLifeTimeOptions<T>(InstanceOptions.DiferentInstances);
        }
    }
}
 