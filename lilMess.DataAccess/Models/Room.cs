namespace lilMess.DataAccess.Models
{
    using MongoRepository;

    [CollectionName("RoomsCollection")]
    public class Room : Entity
    {
        public string Name { get; set; }

        public bool Home { get; set; }

        public Room ParentRoom { get; set; }

        public string Password { get; set; }

        public Permisson[] Permissions { get; set; }
    }
}