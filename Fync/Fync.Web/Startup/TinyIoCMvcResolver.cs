using System;
using System.Collections.Generic;
using System.Linq;
using TinyIoC;

namespace Fync.Common.Libraries
{
     public class TinyIocMvcResolver : System.Web.Mvc.IDependencyResolver  
     {  
         private TinyIoCContainer _container;  
         public TinyIocMvcResolver(TinyIoCContainer container)  
         {  
             _container = container;  
         }  

         public object GetService(Type serviceType)  
         {  
             try  
             {  
                 return _container.Resolve(serviceType);  
             }  
             catch (Exception e)  
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
     }  
}
