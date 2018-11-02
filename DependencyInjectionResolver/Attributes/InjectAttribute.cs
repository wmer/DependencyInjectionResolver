using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionResolver.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class InjectAttribute : Attribute {

    }
}
