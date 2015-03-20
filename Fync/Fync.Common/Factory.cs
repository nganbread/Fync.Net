using System;
using System.Collections.Generic;
using Fync.Common.Common;

namespace Fync.Common
{
    internal class Factory<TOutput> : IFactory<TOutput>
    {
        private readonly Func<string, IDictionary<string, object>, TOutput> _factory;
        private readonly Func<TOutput> _parameterlessFactory;

        public Factory(Func<string, IDictionary<string, object>, TOutput> factory, Func<TOutput> parameterlessFactory)
        {
            _factory = factory;
            _parameterlessFactory = parameterlessFactory;
        }

        public TOutput Manufacture()
        {
            return _parameterlessFactory();
        }

        public TOutput Manufacture(object parameters)
        {
            try
            {
                return _factory(String.Empty, parameters.ToDictionary());
            }
	        catch (Exception e)
	        {
                Logger.Instance.Log(e);
		
		        throw;
	        };
        }
    }
}