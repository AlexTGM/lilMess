namespace lilMess.Client.Models
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    public class RoleModel : ObservableObject
    {
        private string roleName;

        private string roleColor;

        public string Id { get; set; }

        public RoleModel()
        {
            Permissions = new ObservableCollection<PermissionsModel>();
        }

        public string RoleColor
        {
            get { return roleColor; }
            set { Set("RoleColor", ref roleColor, value); }
        }

        public string RoleName
        {
            get { return roleName; }
            set { Set("RoleName", ref roleName, value); }
        }

        public ObservableCollection<PermissionsModel> Permissions { get; set; }
    }
}