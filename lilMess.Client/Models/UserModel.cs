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

        public UserModel()
        {
            
        }

        public RoleModel UserRole
        {
            get { return this.userRole; }
            set { this.Set("UserRole", ref this.userRole, value); }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.Set("UserName", ref this.userName, value); }
        }

        public int Port
        {
            get { return this.port; }
            set { this.Set("Port", ref this.port, value); }
        }

        public bool HasPermittingPermissions(string privelegeName)
        {
            return this.UserRole.Permissions.Where(x => x.PrivilegeName == privelegeName)
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