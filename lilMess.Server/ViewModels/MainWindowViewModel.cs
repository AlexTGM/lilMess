namespace lilMess.Server.ViewModels
{
    using System;
    using System.Text;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using lilMess.Server.Network;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INetwork network;

        private readonly StringBuilder document;

        private string serverInfo;

        public MainWindowViewModel(INetwork network)
        {
            this.document = new StringBuilder();

            this.network = network;
            this.ServerInfo = this.network.StartupServer(this.AddNewParagraph);

            this.ShutdownCommand = new RelayCommand(() => this.network.ShutdownServer());
        }

        public string ServerInfo
        {
            get { return this.serverInfo; }
            set
            {
                this.serverInfo = value;
                this.RaisePropertyChanged();
            }
        }

        public string Document
        {
            get { return this.document.ToString(); }
            set
            {
                this.document.AppendLine(value);
                this.RaisePropertyChanged();
            }
        }

        public RelayCommand ShutdownCommand { get; private set; }

        private void AddNewParagraph(string message)
        {
            this.Document = string.Format("{0}:\n{1}", DateTime.Now, message);
        }
    }
}