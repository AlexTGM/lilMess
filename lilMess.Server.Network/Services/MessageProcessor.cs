namespace lilMess.Server.Network.Services
{
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

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IUserService userService;
        private readonly IRepositoryManager manager;

        private NetServer server;

        public MessageProcessor(IUserService userService, IRepositoryManager manager)
        {
            this.userService = userService;
            this.manager = manager;
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