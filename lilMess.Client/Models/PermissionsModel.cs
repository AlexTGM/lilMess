namespace lilMess.Client.Models
{
    using GalaSoft.MvvmLight;

    public class PermissionsModel : ObservableObject
    {
        private string _privilegeName;

        private bool _permittingPrivilege;

        public string PrivilegeName
        {
            get { return _privilegeName; }
            set { Set("PrivilegeName", ref _privilegeName, value); }
        }

        public bool PermittingPrivilege
        {
            get { return _permittingPrivilege; }
            set { Set("PermittingPrivilege", ref _permittingPrivilege, value); }
        }
    }
}