namespace lilMess.Misc.Model
{
    using System;
    using System.Collections.ObjectModel;

    [Serializable]
    public class RoomModel
    {
        public RoomModel()
        {
            RoomUsers = new ObservableCollection<UserModel>();
        }

        public string Id { get; set; }

        public string RoomName { get; set; }

        public bool RoomIsHome { get; set; }

        public RoomModel RoomParent { get; set; }

        public ObservableCollection<UserModel> RoomUsers { get; set; }
    }
}