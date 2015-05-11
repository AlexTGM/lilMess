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
            ServerInfo = network.StartupServer();

            ShutdownCommand = new RelayCommand(network.ShutdownServer);
            GatherStatisticsCommand = new RelayCommand(() => new StatisticsView().Show());
            
            network.GotMessage += AddNewParagraph;
        }

        public string ServerInfo
        {
            get { return serverInfo; }
            set { Set("ServerInfo", ref serverInfo, value); }
        }

        public string Document
        {
            get { return document.ToString(); }
            set
            {
                document.AppendLine(value);
                RaisePropertyChanged();
            }
        }

        public ICommand ShutdownCommand { get; private set; }

        public ICommand GatherStatisticsCommand { get; private set; }

        private void AddNewParagraph(string message)
        {
            Document = string.Format("{0}:\n{1}", DateTime.Now, message);
        }
    }
}