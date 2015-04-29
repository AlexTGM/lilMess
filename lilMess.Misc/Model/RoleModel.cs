namespace lilMess.Misc.Model
{
    using System;

    [Serializable]
    public class RoleModel
    {
        public string RoleName { get; set; }

        public string RoleColor { get; set; }

        public PermissionModel[] Permissions { get; set; }
    }
}