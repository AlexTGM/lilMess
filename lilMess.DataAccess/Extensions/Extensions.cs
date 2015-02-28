namespace lilMess.DataAccess.Extensions
{
    using System;
    using System.Linq;

    using lilMess.DataAccess.Models;

    using MongoRepository;

    public static class Extensions
    {
        public static User GetOrUpdate(this IRepository<User> repository, string guid, string login)
        {
            var role = new MongoRepository<Role>().FirstOrDefault(x => x.Default);

            if (role == null) { throw new NotImplementedException(); }

            var user = repository.FirstOrDefault(x => x.Guid == guid);

            if (user == null) { user = repository.Add(new User { Guid = guid, Name = login, Role = role }); }
            else if (user.Name != login)
            {
                user.Name = login;
                repository.Update(user);
            }

            return user;
        }
    }
}