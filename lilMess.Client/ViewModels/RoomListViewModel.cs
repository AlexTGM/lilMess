namespace lilMess.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using lilMess.Client.Models;

    public class RoomListViewModel : ViewModelBase
    {
        private ObservableCollection<RoomModel> roomsList = new ObservableCollection<RoomModel>();

        public RoomListViewModel()
        {
            Messenger.Default.Register<NotificationMessage<List<RoomModel>>>(this, MainWindowViewModel.Token, this.UpdateRoomsList);
        }

        public ObservableCollection<RoomModel> RoomsList
        {
            get { return this.roomsList; }
            set { this.Set("RoomsList", ref this.roomsList, value); }
        }

        private void UpdateRoomsList(NotificationMessage<List<RoomModel>> message)
        {
            this.RoomsList = new ObservableCollection<RoomModel>(message.Content);
        }
    }
}