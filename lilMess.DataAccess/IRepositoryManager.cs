namespace lilMess.DataAccess
{
    using lilMess.DataAccess.Models;

    using MongoRepository;

    public interface IRepositoryManager
    {
        IRepository<User> UserRepository { get; }

        IRepository<Role> RoleRepository { get; }

        IRepository<Room> RoomRepository { get; }

        IRepository<Permisson> PermissonRepository { get; }
    }
}