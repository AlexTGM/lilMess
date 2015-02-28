namespace lilMess.DataAccess.Models
{
    using MongoRepository;

    [CollectionName("UsersCollection")]
    public class User : Entity
    {
        public string Name { get; set; }

        public string Guid { get; set; }

        public Role Role { get; set; }
    }
}