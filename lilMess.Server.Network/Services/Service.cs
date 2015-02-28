namespace lilMess.Server.Network.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets;
    using lilMess.Misc.Packets.Body;
    using lilMess.Misc.Requests;
    using lilMess.Server.Network.Models;

    public class Service : IService
    {
        private readonly IUserService userService;

        private readonly NetServer server;

        private readonly Dictionary<PacketType, Func<Request, string>> methods;

        public Service(IUserService userService, NetServer server, IMessageProcessor messageProcessor)
        {
            this.userService = userService;
            this.server = server;

            this.methods = new Dictionary<PacketType, Func<Request, string>>
                               {
                                   { messageProcessor.Chat.Key, messageProcessor.Chat.Value },
                                   { messageProcessor.Audio.Key, messageProcessor.Audio.Value },
                                   { messageProcessor.Connection.Key, messageProcessor.Connection.Value },
                               };

            messageProcessor.SendNewPacket += this.SendPacket;
        }

        public StatisticsModel Stats
        {
            get
            {
                return new StatisticsModel(this.server.Statistics.ReceivedBytes, this.server.Statistics.SentBytes);
            }
        }

        public void StartupServer(ProcessNewMessage processNewMessageDelegate)
        {
            this.server.Start();

            while (this.server.Status == NetPeerStatus.Running)
            {
                var message = this.server.ReadMessage();

                if (message != null) { processNewMessageDelegate.Invoke(message, this.userService.FindUser(message.SenderConnection)); }

                Thread.Sleep(100);
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

        public string StatusChanged(UserModel user, NetIncomingMessage incomingMessage)
        {
            var status = (NetConnectionStatus)incomingMessage.ReadByte();
            if (status == NetConnectionStatus.Disconnected && user != null) { this.userService.RemoveUser(user); }

            var serverInfo = new ServerInfoPacket(new ServerInfoBody { ServerRooms = this.userService.GetRoomsList() });

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(serverInfo), new List<NetConnection>());

            return string.Format("Пользователь {0} теперь имеет статус {1}", incomingMessage.SenderConnection, status);
        }

        private void SendPacket(byte[] data, List<NetConnection> except)
        {
            var recipients = this.server.Connections.Except(except).ToList();

            if (!recipients.Any()) { return; }

            var outgoingMessage = this.server.CreateMessage();

            var sendBuffer = new NetBuffer();
            sendBuffer.Write(data);

            outgoingMessage.Write(sendBuffer);
            this.server.SendMessage(outgoingMessage, recipients, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}