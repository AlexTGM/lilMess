namespace lilMess.Misc.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [Serializable]
    public class RoomModel
    {
        public RoomModel()
        {
            this.Users = new ObservableCollection<UserModel>();
        }

        public string Name { get; set; }

        public bool Home { get; set; }

        public RoomModel Parent { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }
    }
}