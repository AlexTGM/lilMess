﻿namespace lilMess.Client.Models
{
    using GalaSoft.MvvmLight;

    public class UserModel : ObservableObject
    {
        private string userName;

        private RoleModel userRole;

        public RoleModel UserRole
        {
            get { return this.userRole; }
            set { this.Set("userRole", ref this.userRole, value); }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.Set("UserName", ref this.userName, value); }
        }
    }
}