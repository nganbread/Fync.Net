namespace Fync.Service
{
    public interface IAuthenticationService
    {
        bool Login(string emailAddress, string password);
        bool Register(string email, string password);
        void Logout();
    }
}
