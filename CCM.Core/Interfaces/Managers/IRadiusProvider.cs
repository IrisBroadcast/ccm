namespace CCM.Core.Interfaces.Managers
{
    public interface IRadiusProvider
    {
        bool Authenticate(string username, string password);
    }
}