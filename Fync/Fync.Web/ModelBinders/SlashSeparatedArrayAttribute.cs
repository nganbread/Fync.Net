using System.Web.Mvc;

namespace Fync.Web.ModelBinders
{
    public class SlashSeparatedArrayAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new SlashSeparatedArray();
        }
    }
}