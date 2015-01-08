using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using TinyIoC;

namespace Fync.Common.Libraries
{
    public class TinyIocApiResolver : IDependencyResolver
    {
        private readonly TinyIoCContainer _container;
        public TinyIocApiResolver(TinyIoCContainer container)
        {
            _container = container;
        }
        public IDependencyScope BeginScope()
        {
            return this;
        }
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType, true);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }
        public void Dispose()
        {
        }
    }
}