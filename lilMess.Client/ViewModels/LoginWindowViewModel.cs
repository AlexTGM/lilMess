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

            this.LoginModel = new LoginModel();
            this.LoginModel.ErrorsChanged += (sender, args) => this.LoginCommand.RaiseCanExecuteChanged();

            this.LoginCommand = new RelayCommand<object>(this.TryLogin, o => !this.LoginModel.HasErrors);
        }

        public LoginModel LoginModel { get; set; }

        public RelayCommand<object> LoginCommand { get; set; }

        private void TryLogin(object param)
        {
            this.network.Connect(this.LoginModel.Address.Ip, this.LoginModel.Address.Port, this.LoginModel.UserName);

            Messenger.Default.Send(new NotificationMessage(this.LoginModel.ServerInfo), Token);

            ((LoginWindow)param).Hide();
        }
    }
}