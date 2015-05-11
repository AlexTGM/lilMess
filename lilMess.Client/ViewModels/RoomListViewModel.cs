namespace lilMess.Client.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    using AutoMapper;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using GongSolutions.Wpf.DragDrop;

    using lilMess.Client.DragDrop;
    using lilMess.Client.Network;

    using RoomModel = lilMess.Client.Models.RoomModel;

    public class RoomListViewModel : ViewModelBase, IDropTarget
    {
        private readonly INetwork _network;

        private ObservableCollection<RoomModel> _roomsList = new ObservableCollection<RoomModel>();

        public RoomListViewModel(INetwork network)
        {
            _network = network;

            Messenger.Default.Register<NotificationMessage<List<RoomModel>>>(this, MainViewModel.Token, UpdateRoomsList);
        }

        public ObservableCollection<RoomModel> RoomsList
        {
            get { return _roomsList; }
            set { Set("RoomsList", ref _roomsList, value); }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as IDragDropChildenModel;
            var targetItem = dropInfo.TargetItem as IDragDropParentModel;

            if (sourceItem == null || !sourceItem.CanBeDragged || targetItem == null || !targetItem.CanAcceptChildren) return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as IDragDropChildenModel;
            var targetItem = dropInfo.TargetItem as IDragDropParentModel;

            if (targetItem == null) return;

            targetItem.Children.Add(sourceItem);
            ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);

            Misc.Model.RoomModel room = Mapper.Map<Misc.Model.RoomModel>(targetItem);
            Misc.Model.UserModel user = Mapper.Map<Misc.Model.UserModel>(sourceItem);
            
            _network.MoveUser(user, room);
        }

        private void UpdateRoomsList(NotificationMessage<List<RoomModel>> message)
        {
            foreach (var roomModel in message.Content)
            {
                foreach (var vari in roomModel.RoomUsers)
                {
                    roomModel.Children.Add(vari);
                }
            }

            RoomsList = new ObservableCollection<RoomModel>(message.Content);
        }
    }
}