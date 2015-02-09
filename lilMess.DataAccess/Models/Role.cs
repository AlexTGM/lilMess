namespace lilMess.DataAccess.Models
{
    using MongoRepository;

    [CollectionName("RolesColelction")]
    public class Role : Entity
    {
        public string Name { get; set; }
    }
}