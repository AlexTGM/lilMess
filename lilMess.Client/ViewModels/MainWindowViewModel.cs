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

    public class MainWindowViewModel : ViewModelBase
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly KeyboardListener _globalKeyboardHook;

        public static readonly Guid Token = Guid.NewGuid();

        private readonly INetwork _network;

        private readonly IAudioProcessor _audioProcessor;

        private string _serverInfo = "Disconnected", _chatMessage = string.Empty;

        private UserModel _loggedUser;

        public MainWindowViewModel(INetwork clientNetwork, IAudioProcessor audioProcessor)
        {
            _network = clientNetwork;
            _audioProcessor = audioProcessor;

            Messages = new ObservableCollection<ChatMessageModel>();

            SendChatMessageCommand = new RelayCommand(SendTypedMessage, CanSendMessage);
            OpenLoginWindowCommand = new RelayCommand(ShowLoginWindow);
            OpenGitRepositoryCommand = new RelayCommand(OpenGitRepository);

            _network.Chat += message => Messages.Add(Mapper.Map<ChatMessageModel>(message));
            _network.Audio += message => _audioProcessor.Translate(message);
            _network.Refresh += rooms => UpdateRooms(Mapper.Map<List<RoomModel>>(rooms));

            _globalKeyboardHook = new KeyboardListener();
            _globalKeyboardHook.KeyDown += OnGlobalHookKeyDown;
            _globalKeyboardHook.KeyUp += OnGlobalHookKeyUp;

            Messenger.Default.Register<NotificationMessage>(this, LoginWindowViewModel.Token, UpdateConnectionInfo);
        }

        public RelayCommand SendChatMessageCommand { get; private set; }

        public RelayCommand OpenGitRepositoryCommand { get; private set; }

        public RelayCommand OpenLoginWindowCommand { get; private set; }

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
            new LoginWindow().ShowDialog();
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
            _loggedUser = rooms.Select(y => y.RoomUsers.FirstOrDefault(z => z.Port == _network.Port)).FirstOrDefault(y => y != null);

            if (_loggedUser != null)
            {
                _loggedUser.Me = true;
            }

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
            if (_loggedUser == null)
            {
                return false;
            }

            var chatBoxIsNotEmpty = !string.IsNullOrWhiteSpace(ChatMessage);
            var userHavePermission = _loggedUser.HasPermittingPermissions("user_privileges");

            return chatBoxIsNotEmpty && userHavePermission;
        }
    }
}