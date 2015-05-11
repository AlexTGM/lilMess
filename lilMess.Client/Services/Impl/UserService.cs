namespace lilMess.Client.Services.Impl
{
    using System.Collections.Generic;

    using lilMess.Client.Models;

    public class UserService : IUserService
    {
        private readonly List<UserModel> _usersOnline;

        public UserService()
        {
            _usersOnline = new List<UserModel>();
        }

        public UserModel LoggedUser { get; private set; }

        public bool UserHasPermittingPermission(UserModel user, string roleName)
        {
            return user.HasPermittingPermissions(roleName);
        }

        public bool LoggedUserHasPermittingPermission(string roleName)
        {
            return UserHasPermittingPermission(LoggedUser, roleName);
        }

        public void AddNewUser(UserModel user)
        {
            if (_usersOnline.FindIndex(userModel => userModel.Id == user.Id) != -1)
            {
                return;
            }

            _usersOnline.Add(user);
        }

        public UserModel FindUser(string id)
        {
            return _usersOnline.Find(user => user.Port.ToString() == id);
        }

        public void SetClientUser(string id)
        {
            LoggedUser = FindUser(id);
        }
    }
}