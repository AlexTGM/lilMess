namespace lilMess.DataAccess.Extensions
{
    using System.Linq;

    using lilMess.DataAccess.DeployScript;
    using lilMess.DataAccess.Models;

    using MongoRepository;

    public static class Extensions
    {
        public static User GetOrUpdate(this IRepository<User> repository, string guid, string login)
        {
            var roleRepository = new MongoRepository<Role>();

            var user = repository.FirstOrDefault(x => x.Guid == guid);

            if (user == null)
            {
                var role = roleRepository.FirstOrDefault(x => x.Default) ?? Deploy.CreateNewDefaultRole();

                user = repository.Add(new User { Guid = guid, Name = login, Role = role });
            }
            else if (user.Name != login) { user.Name = login; }

            return repository.Update(user);
        }

    }
}