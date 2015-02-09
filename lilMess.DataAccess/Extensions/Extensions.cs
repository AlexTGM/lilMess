namespace lilMess.DataAccess.Extensions
{
    using System.Linq;

    using lilMess.DataAccess.Models;

    using MongoRepository;

    public static class Extensions
    {
        public static User GetOrUpdate(this IRepository<User> repository, string guid, string login)
        {
            var user = repository.FirstOrDefault(x => x.Guid == guid);

            if (user == null)
            {
                repository.Add(new User { Guid = guid, Name = login });
            }
            else if (user.Name != login)
            {
                user.Name = login;
                repository.Update(user);
            }

            return user;
        }
    }
}