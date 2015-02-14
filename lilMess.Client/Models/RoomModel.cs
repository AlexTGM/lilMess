namespace lilMess.Client.Models
{
    using System.Collections.Generic;

    public class RoomModel
    {
        public RoomModel() { this.Users = new List<UserModel>(); }

        public string Name { get; set; }

        public bool Home { get; set; }

        public RoomModel Parent { get; set; }

        public List<UserModel> Users { get; set; }
    }
}