using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Fync.Api.ModelBinders
{
    public class SlashSeparatedArray : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof (string[])) return false;

            bindingContext.Model = actionContext
                .Request
                .RequestUri
                .PathAndQuery
                .Split('/')
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .ToArray();

            return true;
        }
    }
}