namespace lilMess.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using lilMess.Audio;
    using lilMess.Client.Network;
    using lilMess.Client.Views;
    using lilMess.Misc.Model;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INetwork network;

        private readonly IAudioProcessor audioProcessor;

        private readonly StringBuilder document = new StringBuilder("Welcome!");

        private readonly LoginWindow logInWindow;

        private ObservableCollection<RoomModel> roomsList = new ObservableCollection<RoomModel>();

        private string serverInfo = "Disconnected";

        private string chatMessage;

        public MainWindowViewModel(INetwork clientNetwork, IAudioProcessor audioProcessor, LoginWindow logInWindow)
        {
            this.network = clientNetwork;
            this.audioProcessor = audioProcessor;
            this.logInWindow = logInWindow;

            this.network.Initialise(this.DisplayMessage, this.GetRoomsList, this.audioProcessor.Translate);

            this.ShutdownCommand = new RelayCommand(() => this.network.Shutdown());
            this.SendChatMessageCommand = new RelayCommand(
                () =>
                    {
                        this.network.SendChatMessage(this.ChatMessage);
                        this.ChatMessage = string.Empty;
                    });
            this.OpenLoginWindowCommand = new RelayCommand(() => this.logInWindow.ShowDialog());
        }

        public RelayCommand ShutdownCommand { get; private set; }

        public RelayCommand SendChatMessageCommand { get; private set; }
        
        public RelayCommand OpenLoginWindowCommand { get; private set; }

        public string ServerInfo
        {
            get { return this.serverInfo; }
            set
            {
                this.serverInfo = value;
                this.RaisePropertyChanged();
            }
        }

        public string ChatMessage
        {
            get { return this.chatMessage; }
            set
            {
                this.chatMessage = value;
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

        public ObservableCollection<RoomModel> RoomsList
        {
            get { return this.roomsList; }
            set
            {
                this.roomsList = value;
                this.RaisePropertyChanged();
            }
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                this.audioProcessor.StartRecording(this.network.SendVoiceMessage);
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                this.audioProcessor.StopRecording();
            }
        }

        private void DisplayMessage(string message)
        {
            this.Document = message;
        }

        private void GetRoomsList(List<RoomModel> rooms)
        {
            this.RoomsList = new ObservableCollection<RoomModel>(rooms);
        }
    }
}