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
            this.network = clientNetwork;
            this.audioProcessor = audioProcessor;

            this.Messages = new ObservableCollection<ChatMessageModel>();

            this.SendChatMessageCommand = new RelayCommand(this.SendTypedMessage, this.CanSendMessage);
            this.OpenLoginWindowCommand = new RelayCommand(this.ShowLoginWindow);
            this.OpenGitRepositoryCommand = new RelayCommand(this.OpenGitRepository);

            this.network.Chat += message => this.Messages.Add(Mapper.Map<ChatMessageModel>(message));
            this.network.Audio += message => this.audioProcessor.Translate(message);
            this.network.Refresh += rooms => this.UpdateRooms(Mapper.Map<List<RoomModel>>(rooms));

            Messenger.Default.Register<NotificationMessage>(this, LoginWindowViewModel.Token, this.UpdateConnectionInfo);
        }

        public RelayCommand SendChatMessageCommand { get; private set; }

        public RelayCommand OpenGitRepositoryCommand { get; private set; }

        public RelayCommand OpenLoginWindowCommand { get; private set; }

        public ObservableCollection<ChatMessageModel> Messages { get; private set; }

        public string ServerInfo
        {
            get { return this.serverInfo; }
            set { this.Set("ServerInfo", ref this.serverInfo, value); }
        }

        public string ChatMessage
        {
            get { return this.chatMessage; }
            set
            {
                this.Set("ChatMessage", ref this.chatMessage, value);
                this.SendChatMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            this.network.Shutdown();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) { this.audioProcessor.StartRecording(this.network.SendVoiceMessage); }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2) { this.audioProcessor.StopRecording(); }
        }

        private void UpdateRooms(List<RoomModel> rooms)
        {
            this.loggedUser = rooms.Select(y => y.RoomUsers.FirstOrDefault(z => z.Port == this.network.Port)).FirstOrDefault(y => y != null);

            if (this.loggedUser != null) { this.loggedUser.Me = true; }

            Messenger.Default.Send(new NotificationMessage<List<RoomModel>>(rooms, "RoomsWasUpdated"), Token);
        }

        private void SendTypedMessage()
        {
            this.network.SendChatMessage(this.ChatMessage);
            this.ChatMessage = string.Empty;
        }

        private void UpdateConnectionInfo(NotificationMessage message)
        {
            this.ServerInfo = string.Format("Connected to: {0}", message.Notification);
        }

        private void ShowLoginWindow()
        {
            new LoginWindow().ShowDialog();
        }

        private void OpenGitRepository()
        {
            System.Diagnostics.Process.Start("https://github.com/AlexTGM/lilMess");
        }

        private bool CanSendMessage()
        {
            if (this.loggedUser == null) return false;

            var chatBoxIsNotEmpty = !string.IsNullOrWhiteSpace(this.ChatMessage);
            var userHavePermission = this.loggedUser.HasPermittingPermissions("user_privileges");

            return chatBoxIsNotEmpty && userHavePermission;
        }
    }
}