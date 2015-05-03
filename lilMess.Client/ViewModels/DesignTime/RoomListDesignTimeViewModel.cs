namespace lilMess.Client.ViewModels.DesignTime
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    using lilMess.Client.Models;

    public class RoomListDesignTimeViewModel : ViewModelBase
    {
        public ObservableCollection<RoomModel> RoomsList
        {
            get
            {
                var admin = new UserModel
                                    {
                                        Me = true,
                                        Port = 9997,
                                        UserName = "admin",
                                        UserRole = new RoleModel { RoleColor = "Green", RoleName = "Administrator" }
                                    };


                var homeRoom = new RoomModel { RoomIsHome = true, RoomName = "Home Room", RoomParent = null };
                homeRoom.RoomUsers.Add(admin);


                return new ObservableCollection<RoomModel> { homeRoom };
            }
        }
    }
}