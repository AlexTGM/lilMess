namespace lilMess.Misc.Model
{
    using System;

    [Serializable]
    public class PermissionModel
    {
        public string PrivilegeName { get; set; }

        public bool PermittingPrivilege { get; set; }
    }
}