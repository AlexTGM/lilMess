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
    using lilMess.Tools;

    public class MainViewModel : ViewModelBase
    {
        public static readonly Guid Token = Guid.NewGuid();

        private readonly INetwork _network;

        private readonly IAudioProcessor _audioProcessor;

        private readonly ApplicationCommon _applicationCommon;

        private string _serverInfo = "Disconnected", _chatMessage = string.Empty;

        public MainViewModel(INetwork clientNetwork, IAudioProcessor audioProcessor, ApplicationCommon applicationCommon)
        {
            _network = clientNetwork;
            _audioProcessor = audioProcessor;
            _applicationCommon = applicationCommon;

            Messages = new ObservableCollection<ChatMessageModel>();

            SendChatMessageCommand = new RelayCommand(SendTypedMessage, CanSendMessage);
            OpenLoginWindowCommand = new RelayCommand(ShowLoginWindow);
            OpenSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            OpenGitRepositoryCommand = new RelayCommand(OpenGitRepository);

            _network.Chat += message => Messages.Add(Mapper.Map<ChatMessageModel>(message));
            _network.Audio += message => _audioProcessor.Translate(message);
            _network.Refresh += rooms => UpdateRooms(Mapper.Map<List<RoomModel>>(rooms));

            App.KeyboardListener.KeyDown += OnGlobalHookKeyDown;
            App.KeyboardListener.KeyUp += OnGlobalHookKeyUp;

            Messenger.Default.Register<NotificationMessage>(this, LoginViewModel.Token, UpdateConnectionInfo);
        }

        public RelayCommand SendChatMessageCommand { get; private set; }

        public RelayCommand OpenGitRepositoryCommand { get; private set; }

        public RelayCommand OpenLoginWindowCommand { get; private set; }

        public RelayCommand OpenSettingsWindowCommand { get; private set; }

        public ObservableCollection<ChatMessageModel> Messages { get; private set; }

        public string ServerInfo
        {
            get
            {
                return _serverInfo;
            }
            set
            {
                Set("ServerInfo", ref _serverInfo, value);
            }
        }

        public string ChatMessage
        {
            get
            {
                return _chatMessage;
            }
            set
            {
                Set("ChatMessage", ref _chatMessage, value);
                SendChatMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            _network.Shutdown();
        }

        private static void OpenGitRepository()
        {
            System.Diagnostics.Process.Start("https://github.com/AlexTGM/lilMess");
        }

        private static void ShowLoginWindow()
        {
            new LoginView().ShowDialog();
        }        
        
        private static void ShowSettingsWindow()
        {
            new SettingsView().ShowDialog();
        }

        private void OnGlobalHookKeyUp(object sender, RawKeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                _audioProcessor.StopRecording();
            }
        }

        private void OnGlobalHookKeyDown(object sender, RawKeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                _audioProcessor.StartRecording(_network.SendVoiceMessage);
            }
        }

        private void UpdateRooms(List<RoomModel> rooms)
        {
            foreach (var user in rooms.SelectMany(room => room.RoomUsers)) 
            {
                _applicationCommon.UserService.AddNewUser(user);
            }

            _applicationCommon.UserService.SetClientUser(_network.Port.ToString());

            Messenger.Default.Send(new NotificationMessage<List<RoomModel>>(rooms, "RoomsWasUpdated"), Token);
        }

        private void SendTypedMessage()
        {
            _network.SendChatMessage(ChatMessage);
            ChatMessage = string.Empty;
        }

        private void UpdateConnectionInfo(NotificationMessage message)
        {
            ServerInfo = string.Format("Connected to: {0}", message.Notification);
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(ChatMessage) && _applicationCommon.UserService.LoggedUserHasPermittingPermission("user_privileges");
        }
    }
}