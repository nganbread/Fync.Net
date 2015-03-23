using System;
using System.Linq;
using System.Web.Mvc;

namespace Fync.Web.ModelBinders
{
    public class SlashSeparatedArray : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(string[])) return null;

            object path;
            var success = controllerContext
                .RouteData
                .Values
                .TryGetValue("pathComponents", out path);

            if (!success) return null;

            var pathComponents = (path ?? string.Empty)
                .ToString()
                .Split('/')
                .Where(x => !String.IsNullOrWhiteSpace(x));

            return (new[] {"Fync"}).Concat(pathComponents).ToArray();
        }
    }
}