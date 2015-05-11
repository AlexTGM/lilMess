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

        public string Id { get; set; }

        public RoomModel()
        {
            RoomUsers = new ObservableCollection<UserModel>();

            Children = new ObservableCollection<IDragDropChildenModel>();
        }

        public ObservableCollection<UserModel> RoomUsers { get; private set; }

        public ObservableCollection<IDragDropChildenModel> Children { get; private set; }

        public bool CanAcceptChildren { get { return true; } }

        public RoomModel RoomParent
        {
            get { return roomParent; }
            set { Set("RoomParent", ref roomParent, value); }
        }

        public bool RoomIsHome
        {
            get { return roomIsHome; }
            set { Set("RoomIsHome", ref roomIsHome, value); }
        }

        public string RoomName
        {
            get { return roomName; }
            set { Set("RoomName", ref roomName, value); }
        }

        public bool CanBeDragged
        {
            get { return false; }
            set { throw new NotImplementedException(); }
        }
    }
}