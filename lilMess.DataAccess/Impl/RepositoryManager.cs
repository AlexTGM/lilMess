namespace lilMess.DataAccess.Impl
{
    using lilMess.DataAccess.Models;

    using MongoRepository;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IRepository<User> userRepository = new MongoRepository<User>();

        private readonly IRepository<Role> roleRepository = new MongoRepository<Role>();

        private readonly IRepository<Room> roomRepository = new MongoRepository<Room>();

        private readonly IRepository<Permisson> permissionRepository = new MongoRepository<Permisson>();

        public IRepository<User> UserRepository
        {
            get { return userRepository; }
        }

        public IRepository<Role> RoleRepository
        {
            get { return roleRepository; }
        }

        public IRepository<Room> RoomRepository
        {
            get { return roomRepository; }
        }

        public IRepository<Permisson> PermissonRepository
        {
            get { return permissionRepository; }
        }
    }
}