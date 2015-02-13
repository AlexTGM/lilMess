namespace lilMess.Client.ViewModels
{
    using System;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using lilMess.Client.Network;
    using lilMess.Client.Views;
    using lilMess.Tools;

    public class LoginWindowViewModel : ViewModelBase
    {
        private readonly INetwork network;

        private string serverInfo = "127.0.0.1:9997";

        private string userName = "admin";

        public LoginWindowViewModel(INetwork network)
        {
            this.network = network;

            this.LoginCommand = new RelayCommand<object>(this.TryLogin);
        }

        public RelayCommand<object> LoginCommand { get; set; }

        public string ServerInfo
        {
            get { return this.serverInfo; }
            set { this.Set("ServerInfo", ref this.serverInfo, value); }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.Set("UserName", ref this.userName, value); }
        }

        private void TryLogin(object param)
        {
            var ep = AddressParser.ParseHostPort(this.ServerInfo);
            if (ep == null) throw new Exception("Проверьте правильность введенного адреса");

            Task.Factory.StartNew(() => this.network.Connect(ep.Address.ToString(), ep.Port, this.UserName)).Wait();

            ((LoginWindow)param).Hide();
        }
    }
}