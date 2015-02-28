namespace lilMess.Server.ViewModels
{
    using System;
    using System.Text;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using lilMess.Server.Network;
    using lilMess.Server.Views;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly StringBuilder document = new StringBuilder();

        private string serverInfo;

        public MainWindowViewModel(INetwork network)
        {
            this.ServerInfo = network.StartupServer();

            this.ShutdownCommand = new RelayCommand(network.ShutdownServer);
            this.GatherStatisticsCommand = new RelayCommand(() => new StatisticsView().Show());
            
            network.GotMessage += this.AddNewParagraph;
        }

        public string ServerInfo
        {
            get { return this.serverInfo; }
            set { this.Set("ServerInfo", ref this.serverInfo, value); }
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

        public ICommand ShutdownCommand { get; private set; }

        public ICommand GatherStatisticsCommand { get; private set; }

        private void AddNewParagraph(string message)
        {
            this.Document = string.Format("{0}:\n{1}", DateTime.Now, message);
        }
    }
}