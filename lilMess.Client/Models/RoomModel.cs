namespace lilMess.Client.Models
{
    using System;
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    using lilMess.Client.DragDrop;

    public class RoomModel : ObservableObject, IDragDropParentModel
    {
        private string roomName;

        private bool roomIsHome;

        private RoomModel roomParent;

        public RoomModel()
        {
            this.RoomUsers = new ObservableCollection<UserModel>();

            this.Children = new ObservableCollection<IDragDropChildenModel>();
        }

        public ObservableCollection<UserModel> RoomUsers { get; private set; }

        public ObservableCollection<IDragDropChildenModel> Children { get; private set; }

        public bool CanAcceptChildren { get { return true; } }

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

        public bool CanBeDragged
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }
    }
}