using System.Web.Http.ModelBinding;

namespace Fync.Api.ModelBinders
{
    public class SlashSeparatedArrayAttribute : ModelBinderAttribute
    {
        public SlashSeparatedArrayAttribute()
            : base(typeof(SlashSeparatedArrayModelBinder))
        {

        }
    }
}