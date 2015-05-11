namespace lilMess.Server.Network.Services.Impl
{
    using lilMess.DataAccess;
    using lilMess.DataAccess.Extensions;
    using lilMess.DataAccess.Models;

    public class UserService : IUserService
    {
        private readonly IRepositoryManager _manager;

        public UserService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public User GetOrUpdate(string guid, string login)
        {
            return _manager.UserRepository.GetOrUpdate(guid, login);
        }
    }
}