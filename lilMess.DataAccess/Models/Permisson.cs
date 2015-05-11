namespace lilMess.DataAccess.Models
{
    using MongoRepository;

    [CollectionName("PermissionsCollection")]
    public class Permisson : Entity
    {
        public string PrivilegeName { get; set; }

        public bool PermittingPrivilege { get; set; }
    }
}