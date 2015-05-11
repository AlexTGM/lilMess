namespace lilMess.Client.ViewModels
{
    using System;
    using System.Globalization;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using lilMess.Client.Models;
    using lilMess.Client.Network;
    using lilMess.Client.Views;

    public class LoginViewModel : ViewModelBase
    {
        public static readonly Guid Token = Guid.NewGuid();

        private readonly INetwork _network;

        public LoginViewModel(INetwork network)
        {
            _network = network;

            LoginModel = new LoginModel();
            LoginModel.ErrorsChanged += (sender, args) => LoginCommand.RaiseCanExecuteChanged();

            LoginCommand = new RelayCommand<object>(TryLogin, o => !LoginModel.HasErrors);
        }

        public LoginModel LoginModel { get; set; }

        public RelayCommand<object> LoginCommand { get; set; }

        private void TryLogin(object param)
        {
            _network.Connect(LoginModel.Address.Ip, LoginModel.Address.Port, LoginModel.UserName);

            Messenger.Default.Send(new NotificationMessage(LoginModel.ServerInfo), Token);

            ((LoginView)param).Hide();
        }
    }
}