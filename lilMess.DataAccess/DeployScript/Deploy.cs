namespace lilMess.DataAccess.DeployScript
{
    using lilMess.DataAccess.Models;

    using MongoRepository;

    public static class Deploy
    {
        public static Role CreateNewDefaultRole()
        {
            var permission = new Permisson { PermittingPrivilege = true, PrivilegeName = "user_privileges" };

            var role = new Role { Color = "Black", Default = true, Name = "Users", Permissions = new[] { permission } };

            return new MongoRepository<Role>().Update(role);
        }
    }
}