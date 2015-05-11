namespace lilMess.Client.Models
{
    using System.Linq;

    using GalaSoft.MvvmLight;

    using lilMess.Client.DragDrop;

    public class UserModel : ObservableObject, IDragDropChildenModel
    {
        private string _userName;

        private RoleModel _userRole;

        private int _port;

        public string Id { get; set; }

        public RoleModel UserRole
        {
            get { return _userRole; }
            set { Set("UserRole", ref _userRole, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { Set("UserName", ref _userName, value); }
        }

        public int Port
        {
            get { return _port; }
            set { Set("Port", ref _port, value); }
        }

        public bool HasPermittingPermissions(string privelegeName)
        {
            return UserRole.Permissions.Where(x => x.PrivilegeName == privelegeName)
                                            .Select(x => x.PermittingPrivilege)
                                            .FirstOrDefault();
        }
        
        public bool CanBeDragged
        {
            get { return true; }
            set { throw new System.NotImplementedException(); }
        }
    }
}