namespace lilMess.Client.Models
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    public class RoomModel : ObservableObject
    {
        private string roomName;

        private bool roomIsHome;

        private RoomModel roomParent;

        public RoomModel()
        {
            this.RoomUsers = new ObservableCollection<UserModel>();
        }

        public ObservableCollection<UserModel> RoomUsers { get; private set; }

        public RoomModel RoomParent
        {
            get { return this.roomParent; }
            set { this.Set("RoomParent", ref this.roomParent, value); }
        }

        public bool RoomIsHome
        {
            get { return this.roomIsHome; }
            set { this.Set("RoomIsHome", ref this.roomIsHome, value); }
        }

        public string RoomName
        {
            get { return this.roomName; }
            set { this.Set("RoomName", ref this.roomName, value); }
        }
    }
}