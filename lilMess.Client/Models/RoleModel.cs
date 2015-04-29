namespace lilMess.Client.Models
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    public class RoleModel : ObservableObject
    {
        private string roleName;

        private string roleColor;

        public RoleModel()
        {
            this.Permissions = new ObservableCollection<PermissionsModel>();
        }

        public string RoleColor
        {
            get { return this.roleColor; }
            set { this.Set("RoleColor", ref this.roleColor, value); }
        }

        public string RoleName
        {
            get { return this.roleName; }
            set { this.Set("RoleName", ref this.roleName, value); }
        }

        public ObservableCollection<PermissionsModel> Permissions { get; set; }
    }
}