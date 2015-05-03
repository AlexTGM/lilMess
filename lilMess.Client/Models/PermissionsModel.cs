namespace lilMess.Client.Models
{
    using GalaSoft.MvvmLight;

    public class PermissionsModel : ObservableObject
    {
        private string privilegeName;

        private bool permittingPrivilege;

        public string PrivilegeName
        {
            get { return privilegeName; }
            set { Set("PrivilegeName", ref privilegeName, value); }
        }

        public bool PermittingPrivilege
        {
            get { return permittingPrivilege; }
            set { Set("PermittingPrivilege", ref permittingPrivilege, value); }
        }
    }
}