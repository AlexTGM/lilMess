namespace lilMess.Server.Network.Services.Impl
{
    using lilMess.DataAccess;
    using lilMess.DataAccess.Extensions;
    using lilMess.DataAccess.Models;

    public class UserService : IUserService
    {
        private readonly IRepositoryManager manager;

        public UserService(IRepositoryManager manager)
        {
            this.manager = manager;
        }

        public User GetOrUpdate(string guid, string login)
        {
            return manager.UserRepository.GetOrUpdate(guid, login);
        }
    }
}