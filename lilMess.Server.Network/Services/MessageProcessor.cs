namespace lilMess.Server.Network.Services
{
    using System;
    using System.Collections.Generic;

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

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IUserService userService;
        private readonly IRepositoryManager manager;

        public MessageProcessor(IUserService userService, IRepositoryManager manager)
        {
            this.userService = userService;
            this.manager = manager;

            this.Chat = new KeyValuePair<PacketType, Func<Request, string>>(
                 PacketType.ChatMessage,
                 request => this.GetChatMessage(request.UserModel, ((ChatMessageBody)request.Body).Message));

            this.Audio = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.VoiceMessage,
                request => this.GetVoiceMessage(request.UserModel, ((VoiceMessageBody)request.Body).Message));

            this.Connection = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.LogIn,
                request =>
                {
                    var body = (AuthenticationBody)request.Body;
                    return this.ConnectionApproval(request.IncomingMessage, body.Guid, body.Login);
                });
        }

        public SendNewPacket SendNewPacket { get; set; }

        public KeyValuePair<PacketType, Func<Request, string>> Chat { get; set; }

        public KeyValuePair<PacketType, Func<Request, string>> Audio { get; set; }

        public KeyValuePair<PacketType, Func<Request, string>> Connection { get; set; }

        public string ConnectionApproval(NetIncomingMessage incomingMessage, string guid, string login)
        {
            var user = this.manager.UserRepository.GetOrUpdate(guid, login);

            incomingMessage.SenderConnection.Approve();

            var userModel = Mapper.Map<User, UserModel>(user);
            userModel.Connection = incomingMessage.SenderConnection;

            this.userService.AddUser(userModel);

            return string.Format("Подключился пользователь {0}", login);
        }

        public string GetChatMessage(UserModel user, string message)
        {
            var sendNewPacket = SendNewPacket;

            var chatMessage = new ChatMessagePacket(new ChatMessageBody { Sender = user.Name, Message = message });
            if (sendNewPacket != null) { sendNewPacket(Serializer<ChatMessagePacket>.SerializeObject(chatMessage), new List<NetConnection>()); }

            return string.Format("Сообщение от {0}: {1}", user.Name, message);
        }

        public string GetVoiceMessage(UserModel user, byte[] message)
        {
            var sendNewPacket = SendNewPacket;

            var except = new List<NetConnection> { user.Connection };

            var voiceMessage = new VoiceMessagePacket(new VoiceMessageBody { Message = message });
            if (sendNewPacket != null) { sendNewPacket(Serializer<ChatMessagePacket>.SerializeObject(voiceMessage), except); }

            return string.Format("Пользователь {0} начал запись голосового сообщения", user.Name);
        }
    }
}