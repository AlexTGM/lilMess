namespace lilMess.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using AutoMapper;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using lilMess.Audio;
    using lilMess.Client.Models;
    using lilMess.Client.Network;
    using lilMess.Client.Views;

    public class MainWindowViewModel : ViewModelBase
    {
        public static readonly Guid Token = Guid.NewGuid();

        private readonly INetwork network;

        private readonly IAudioProcessor audioProcessor;

        private string serverInfo = "Disconnected", chatMessage = string.Empty;

        private UserModel loggedUser;

        public MainWindowViewModel(INetwork clientNetwork, IAudioProcessor audioProcessor)
        {
            network = clientNetwork;
            this.audioProcessor = audioProcessor;

            Messages = new ObservableCollection<ChatMessageModel>();

            SendChatMessageCommand = new RelayCommand(SendTypedMessage, CanSendMessage);
            OpenLoginWindowCommand = new RelayCommand(ShowLoginWindow);
            OpenGitRepositoryCommand = new RelayCommand(OpenGitRepository);

            network.Chat += message => Messages.Add(Mapper.Map<ChatMessageModel>(message));
            network.Audio += message => this.audioProcessor.Translate(message);
            network.Refresh += rooms => UpdateRooms(Mapper.Map<List<RoomModel>>(rooms));

            Messenger.Default.Register<NotificationMessage>(this, LoginWindowViewModel.Token, UpdateConnectionInfo);
        }

        public RelayCommand SendChatMessageCommand { get; private set; }

        public RelayCommand OpenGitRepositoryCommand { get; private set; }

        public RelayCommand OpenLoginWindowCommand { get; private set; }

        public ObservableCollection<ChatMessageModel> Messages { get; private set; }

        public string ServerInfo
        {
            get { return serverInfo; }
            set { Set("ServerInfo", ref serverInfo, value); }
        }

        public string ChatMessage
        {
            get { return chatMessage; }
            set
            {
                Set("ChatMessage", ref chatMessage, value);
                SendChatMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            network.Shutdown();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) { audioProcessor.StartRecording(network.SendVoiceMessage); }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) { audioProcessor.StopRecording(); }
        }

        private static void OpenGitRepository()
        {
            System.Diagnostics.Process.Start("https://github.com/AlexTGM/lilMess");
        }

        private static void ShowLoginWindow()
        {
            new LoginWindow().ShowDialog();
        }

        private void UpdateRooms(List<RoomModel> rooms)
        {
            loggedUser = rooms.Select(y => y.RoomUsers.FirstOrDefault(z => z.Port == network.Port)).FirstOrDefault(y => y != null);

            if (loggedUser != null) { loggedUser.Me = true; }

            Messenger.Default.Send(new NotificationMessage<List<RoomModel>>(rooms, "RoomsWasUpdated"), Token);
        }

        private void SendTypedMessage()
        {
            network.SendChatMessage(ChatMessage);
            ChatMessage = string.Empty;
        }

        private void UpdateConnectionInfo(NotificationMessage message)
        {
            ServerInfo = string.Format("Connected to: {0}", message.Notification);
        }

        private bool CanSendMessage()
        {
            if (loggedUser == null) return false;

            var chatBoxIsNotEmpty = !string.IsNullOrWhiteSpace(ChatMessage);
            var userHavePermission = loggedUser.HasPermittingPermissions("user_privileges");

            return chatBoxIsNotEmpty && userHavePermission;
        }
    }
}