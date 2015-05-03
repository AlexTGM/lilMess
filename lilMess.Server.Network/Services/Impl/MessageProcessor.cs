namespace lilMess.Server.Network.Services.Impl
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess.Models;
    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets;
    using lilMess.Misc.Packets.Body;
    using lilMess.Misc.Requests;

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IRoomService _roomService;

        private readonly IUserService _userService;

        public MessageProcessor(IRoomService roomService, IUserService userService)
        {
            _roomService = roomService;
            _userService = userService;

            Chat = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.ChatMessage,
                request => GetChatMessage(request.UserModel, ((ChatMessageBody)request.Body).ChatMessageModel.MessageContent));

            Audio = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.VoiceMessage,
                request => GetVoiceMessage(request.UserModel, ((VoiceMessageBody)request.Body).Message));

            Connection = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.LogIn,
                request => ConnectionApproval(request.IncomingMessage, (AuthenticationBody)request.Body));

            Move = new KeyValuePair<PacketType, Func<Request, string>>(
                PacketType.ServerMessage,
                request => MoveUser(request.UserModel, (AuthenticationBody)request.Body));
        }

        public SendNewPacket SendNewPacket { get; set; }

        public KeyValuePair<PacketType, Func<Request, string>> Chat { get; private set; }

        public KeyValuePair<PacketType, Func<Request, string>> Audio { get; private set; }

        public KeyValuePair<PacketType, Func<Request, string>> Connection { get; private set; }

        public KeyValuePair<PacketType, Func<Request, string>> Move { get; private set; } 

        public string ConnectionApproval(NetIncomingMessage incomingMessage, AuthenticationBody body)
        {
            var user = _userService.GetOrUpdate(body.Guid, body.Login);

            incomingMessage.SenderConnection.Approve();

            var userModel = Mapper.Map<User, UserModel>(user);
            userModel.Connection = incomingMessage.SenderConnection;

            _roomService.AddUser(userModel);

            return string.Format("Подключился пользователь {0}", body.Login);
        }

        public string GetChatMessage(UserModel user, string message)
        {
            var sendNewPacket = SendNewPacket;
            if (sendNewPacket == null) return string.Empty;

            var chatMessageModel = new ChatMessageModel { MessageContent = message, MessageSender = user };
            var chatMessage = new ChatMessagePacket(new ChatMessageBody { ChatMessageModel = chatMessageModel });

            sendNewPacket(Serializer<ChatMessagePacket>.SerializeObject(chatMessage), user);

            return string.Format("Сообщение от {0}: {1}", user.UserName, message);
        }

        public string GetVoiceMessage(UserModel user, byte[] message)
        {
            var sendNewPacket = SendNewPacket;
            if (sendNewPacket == null) return string.Empty;

            var except = new List<NetConnection> { user.Connection };

            var voiceMessage = new VoiceMessagePacket(new VoiceMessageBody { Message = message });
            sendNewPacket(Serializer<ChatMessagePacket>.SerializeObject(voiceMessage), user, except);

            return string.Format("Пользователь {0} начал запись голосового сообщения", user.UserName);
        }

        public string MoveUser(UserModel user, RoomModel destinationRoom)
        {
            var sendNewPacket = SendNewPacket;
            if (sendNewPacket == null) return string.Empty;

            _roomService.MoveUserToRoom(user, destinationRoom);

            var serverInfo = new ServerInfoPacket(new ServerInfoBody { ServerRooms = _roomService.RoomList });

            sendNewPacket(Serializer<ChatMessagePacket>.SerializeObject(serverInfo));

            return string.Format("Пользователь {0} теперь находится в канале {1}", user.UserName, destinationRoom.RoomName);
        }
    }
}