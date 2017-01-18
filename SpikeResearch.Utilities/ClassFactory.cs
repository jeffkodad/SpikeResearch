using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontPanic.Helpers;

namespace SpikeResearch.Utilities
{
    public static class ClassFactory
    {
        private static readonly ProxyFactory Factory = new ProxyFactory();

        public static T CreateClass<T>() where T : class
        {
            return Factory.Proxy<T>();
        }

        public static void OverrideProxy<TContract, TImpl>(TImpl impl)
            where TContract : class
            where TImpl : TContract
        {
            Factory.AddProxyOverride<TContract>(impl);
        }
    }
}
