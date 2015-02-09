namespace lilMess.DataAccess.Impl
{
    using lilMess.DataAccess.Models;

    using MongoRepository;

    public class RepositoryManager : IRepositoryManager
    {
        private readonly IRepository<User> userRepository = new MongoRepository<User>();

        private readonly IRepository<Role> roleRepository = new MongoRepository<Role>();

        private readonly IRepository<Room> roomRepository = new MongoRepository<Room>();

        public IRepository<User> UserRepository { get { return this.userRepository; } }

        public IRepository<Role> RoleRepository { get { return this.roleRepository; } }

        public IRepository<Room> RoomRepository { get { return this.roomRepository; } }
    }
}