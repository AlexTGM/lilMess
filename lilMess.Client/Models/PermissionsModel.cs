namespace lilMess.Client.Models
{
    using GalaSoft.MvvmLight;

    public class PermissionsModel : ObservableObject
    {
        private string privilegeName;

        private bool permittingPrivilege;

        public PermissionsModel()
        {
        }

        public string PrivilegeName
        {
            get { return this.privilegeName; }
            set { this.Set("PrivilegeName", ref this.privilegeName, value); }
        }

        public bool PermittingPrivilege
        {
            get { return this.permittingPrivilege; }
            set { this.Set("PermittingPrivilege", ref this.permittingPrivilege, value); }
        }
    }
}