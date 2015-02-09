namespace lilMess.Server.Network.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess;
    using lilMess.DataAccess.Extensions;
    using lilMess.DataAccess.Models;
    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets;
    using lilMess.Misc.Packets.Body;
    using lilMess.Misc.Requests;

    public class Service : IService
    {
        private readonly IRepositoryManager manager;
        private readonly IUserService userService;

        private NetServer server;

        private Dictionary<PacketType, Func<Request, string>> methods;

        public Service(IRepositoryManager manager, IUserService userService)
        {
            this.manager = manager;
            this.userService = userService;

            this.Initialise();
        }

        public void StartupServer(ProcessNewMessage processNewMessageDelegate)
        {
            this.server.Start();

            while (this.server.Status == NetPeerStatus.Running)
            {
                var message = this.server.ReadMessage();

                if (message == null) continue;

                processNewMessageDelegate.Invoke(message, this.userService.FindUser(message.SenderConnection));
            }
        }

        public string InvokeMethod(Packet packet, Request request)
        {
            return this.methods[(PacketType)packet.PacketType].Invoke(request);
        }

        public void ShutdownServer()
        {
            this.server.Shutdown("Сервер отключен администратором");
        }

        public string ConnectionApproval(NetIncomingMessage incomingMessage, string guid, string login)
        {
            var user = this.manager.UserRepository.GetOrUpdate(guid, login);

            incomingMessage.SenderConnection.Approve();

            var userModel = Mapper.Map<User, UserModel>(user);
            userModel.Connection = incomingMessage.SenderConnection;

            this.userService.AddUser(userModel);

            return string.Format("Подключился пользователь {0}", login);
        }

        public string StatusChanged(UserModel user, NetIncomingMessage incomingMessage)
        {
            var status = (NetConnectionStatus)incomingMessage.ReadByte();
            if (status == NetConnectionStatus.Disconnected && user != null) this.userService.RemoveUser(user);

            var serverInfoBody = new ServerInfoBody { Rooms = this.userService.GetRoomsList() };
            var serverInfo = new ServerInfoPacket(serverInfoBody);

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(serverInfo), this.server.Connections);

            return string.Format("Пользователь {0} теперь имеет статус {1}", incomingMessage.SenderConnection, status);
        }

        public string GetChatMessage(UserModel user, string message)
        {
            var messageBody = new ChatMessageBody { Sender = user.Name, Message = message };
            var chatMessage = new ChatMessagePacket(messageBody);

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(chatMessage), this.server.Connections);

            return string.Format("Сообщение от {0}: {1}", user.Name, message);
        }

        public string GetVoiceMessage(UserModel user, byte[] message)
        {
            var recipients = this.server.Connections.Where(x => x != user.Connection).ToList();

            var messageBody = new VoiceMessageBody { Message = message };
            var voiceMessage = new VoiceMessagePacket(messageBody);

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(voiceMessage), recipients);

            return string.Format("Пользователь {0} начал запись голосового сообщения", user.Name);
        }

        private void Initialise()
        {
            var chatMessageMethod = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.ChatMessage,
                request => this.GetChatMessage(request.UserModel, ((ChatMessageBody)request.Body).Message));

            var authenticationMethod = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.LogIn,
                request =>
                    {
                        var body = (AuthenticationBody)request.Body;
                        return this.ConnectionApproval(request.IncomingMessage, body.Guid, body.Login);
                    });

            var voiceMessageMethod = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.VoiceMessage,
                request => this.GetVoiceMessage(request.UserModel, ((VoiceMessageBody)request.Body).Message));

            this.methods = new Dictionary<PacketType, Func<Request, string>>
                               {
                                   {
                                       chatMessageMethod.Key,
                                       chatMessageMethod.Value
                                   },
                                   {
                                       authenticationMethod.Key,
                                       authenticationMethod.Value
                                   },
                                   {
                                       voiceMessageMethod.Key,
                                       voiceMessageMethod.Value
                                   }
                               };

            var config = new NetPeerConfiguration("lilMess") { MaximumConnections = 100, Port = 9997 };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.server = new NetServer(config);
        }

        private void SendPacket(byte[] data, List<NetConnection> recipients)
        {
            if (!recipients.Any()) return;

            var outgoingMessage = this.server.CreateMessage();

            var sendBuffer = new NetBuffer();
            sendBuffer.Write(data);

            outgoingMessage.Write(sendBuffer);

            this.server.SendMessage(outgoingMessage, recipients, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}