using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Fync.Common;

namespace Fync.Api.ModelBinders
{
    internal class SlashSeparatedArrayModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof (string[])) return false;

            bindingContext.Model = actionContext
                .Request
                .RequestUri
                .PathAndQuery
                .Unescape()
                .Split('/')
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Skip(1) //dont want the controller name
                .ToArray();

            return true;
        }
    }
}