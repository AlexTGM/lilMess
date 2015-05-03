namespace lilMess.Client.Models
{
    using System.Linq;

    using GalaSoft.MvvmLight;

    using lilMess.Client.DragDrop;

    public class UserModel : ObservableObject, IDragDropChildenModel
    {
        private string userName;

        private RoleModel userRole;

        private int port;

        public RoleModel UserRole
        {
            get { return userRole; }
            set { Set("UserRole", ref userRole, value); }
        }

        public string UserName
        {
            get { return userName; }
            set { Set("UserName", ref userName, value); }
        }

        public int Port
        {
            get { return port; }
            set { Set("Port", ref port, value); }
        }

        public bool HasPermittingPermissions(string privelegeName)
        {
            return UserRole.Permissions.Where(x => x.PrivilegeName == privelegeName)
                                            .Select(x => x.PermittingPrivilege)
                                            .FirstOrDefault();
        }

        public bool Me { get; set; }

        public bool CanBeDragged
        {
            get { return true; }
            set { throw new System.NotImplementedException(); }
        }
    }
}