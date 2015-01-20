using Fync.Data.Identity;

namespace Fync.Service
{
    public interface ICurrentUser
    {
        User User { get; }
    }
}
