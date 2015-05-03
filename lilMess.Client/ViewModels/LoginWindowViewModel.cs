namespace lilMess.Client.ViewModels
{
    using System;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using lilMess.Client.Models;
    using lilMess.Client.Network;
    using lilMess.Client.Views;

    public class LoginWindowViewModel : ViewModelBase
    {
        public static readonly Guid Token = Guid.NewGuid();

        private readonly INetwork network;

        public LoginWindowViewModel(INetwork network)
        {
            this.network = network;

            LoginModel = new LoginModel();
            LoginModel.ErrorsChanged += (sender, args) => LoginCommand.RaiseCanExecuteChanged();

            LoginCommand = new RelayCommand<object>(TryLogin, o => !LoginModel.HasErrors);
        }

        public LoginModel LoginModel { get; set; }

        public RelayCommand<object> LoginCommand { get; set; }

        private void TryLogin(object param)
        {
            network.Connect(LoginModel.Address.Ip, LoginModel.Address.Port, LoginModel.UserName);

            Messenger.Default.Send(new NotificationMessage(LoginModel.ServerInfo), Token);

            ((LoginWindow)param).Hide();
        }
    }
}