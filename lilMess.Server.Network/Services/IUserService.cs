namespace lilMess.Server.Network.Services
{
    using lilMess.DataAccess.Models;

    public interface IUserService
    {
        User GetOrUpdate(string guid, string login);
    }
}