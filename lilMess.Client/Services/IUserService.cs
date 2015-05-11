namespace lilMess.Client.Services
{
    using lilMess.Client.Models;

    public interface IUserService
    {
        UserModel LoggedUser { get; }

        bool UserHasPermittingPermission(UserModel user, string roleName);

        bool LoggedUserHasPermittingPermission(string roleName);

        void AddNewUser(UserModel user);

        UserModel FindUser(string id);

        void SetClientUser(string id);
    }
}