namespace lilMess.DataAccess.Models
{
    using MongoRepository;

    [CollectionName("RolesColelction")]
    public class Role : Entity
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public bool Default { get; set; }
    }
}