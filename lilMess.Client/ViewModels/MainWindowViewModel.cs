namespace lilMess.Client.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;

    using AutoMapper;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using lilMess.Audio;
    using lilMess.Client.Models;
    using lilMess.Client.Network;
    using lilMess.Client.Views;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INetwork network;

        private readonly IAudioProcessor audioProcessor;

        private readonly StringBuilder document = new StringBuilder("Welcome!");

        private ObservableCollection<RoomModel> roomsList = new ObservableCollection<RoomModel>();

        private string serverInfo = "Disconnected";

        private string chatMessage;

        public MainWindowViewModel(INetwork clientNetwork, IAudioProcessor audioProcessor)
        {
            this.network = clientNetwork;
            this.audioProcessor = audioProcessor;

            this.network.StartClient();

            this.ShutdownCommand = new RelayCommand(() => this.network.Shutdown());
            this.SendChatMessageCommand = new RelayCommand(
                () =>
                    {
                        this.network.SendChatMessage(this.ChatMessage);
                        this.ChatMessage = string.Empty;
                    });
            this.OpenLoginWindowCommand = new RelayCommand(() => new LoginWindow().ShowDialog());

            this.network.Chat += message => this.Document = message;
            this.network.Audio += message => this.audioProcessor.Translate(message);
            this.network.Refresh += rooms =>
                {
                    var mapped = Mapper.Map<ObservableCollection<RoomModel>>(rooms);
                    this.RoomsList = new ObservableCollection<RoomModel>(mapped);
                };
        }

        public RelayCommand ShutdownCommand { get; private set; }

        public RelayCommand SendChatMessageCommand { get; private set; }
        
        public RelayCommand OpenLoginWindowCommand { get; private set; }
        
        public string ServerInfo
        {
            get { return this.serverInfo; }
            set { this.Set("ServerInfo", ref this.serverInfo, value); }
        }

        public string ChatMessage
        {
            get { return this.chatMessage; }
            set { this.Set("ChatMessage", ref this.chatMessage, value); }
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
            set { this.Set("RoomsList", ref this.roomsList, value); }
        }

        public void OnWindowClosing(object sender, EventArgs e) { this.network.Shutdown(); }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) this.audioProcessor.StartRecording(this.network.SendVoiceMessage);
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) this.audioProcessor.StopRecording();
        }
    }
}