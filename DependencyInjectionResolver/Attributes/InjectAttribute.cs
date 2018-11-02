using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionResolver.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public class InjectAttribute : Attribute {

    }
}
