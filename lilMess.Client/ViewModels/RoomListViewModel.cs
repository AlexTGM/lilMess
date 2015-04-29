namespace lilMess.Client.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using GongSolutions.Wpf.DragDrop;

    using lilMess.Client.DragDrop;
    using lilMess.Client.Models;

    public class RoomListViewModel : ViewModelBase, IDropTarget
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

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as IDragDropChildenModel;
            var targetItem = dropInfo.TargetItem as IDragDropParentModel;

            if (sourceItem == null || targetItem == null || !targetItem.CanAcceptChildren) return;

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

            this.RoomsList = new ObservableCollection<RoomModel>(message.Content);
        }
    }
}