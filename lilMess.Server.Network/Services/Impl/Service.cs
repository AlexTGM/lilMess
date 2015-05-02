namespace lilMess.Server.Network.Services.Impl
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

    public class Service : IService
    {
        private readonly IRoomService roomService;

        private readonly NetServer server;

        private readonly Dictionary<PacketType, Func<Request, string>> methods;

        public Service(IRoomService roomService, NetServer server, IMessageProcessor messageProcessor)
        {
            this.roomService = roomService;
            this.server = server;

            this.methods = new Dictionary<PacketType, Func<Request, string>>
                               {
                                   { messageProcessor.Chat.Key, messageProcessor.Chat.Value },
                                   { messageProcessor.Audio.Key, messageProcessor.Audio.Value },
                                   { messageProcessor.Connection.Key, messageProcessor.Connection.Value },
                               };

            messageProcessor.SendNewPacket += this.SendPacket;
        }
        public void StartupServer(ProcessNewMessage processNewMessageDelegate)
        {
            this.server.Start();

            while (this.server.Status == NetPeerStatus.Running)
            {
                var message = this.server.ReadMessage();

                if (message != null) { processNewMessageDelegate.Invoke(message, this.roomService.FindUser(message.SenderConnection)); }

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
            if (status == NetConnectionStatus.Disconnected && user != null) { this.roomService.RemoveUser(user); }

            var serverInfo = new ServerInfoPacket(new ServerInfoBody { ServerRooms = this.roomService.RoomList });

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(serverInfo));

            return string.Format("Пользователь {0} теперь имеет статус {1}", incomingMessage.SenderConnection, status);
        }

        private void SendPacket(byte[] data, UserModel sender = null, List<NetConnection> except = null)
        {
            var userCurrentRoom = this.roomService.GetUserCurrentRoom(sender);

            var connectionsList = userCurrentRoom == null 
                ? this.server.Connections.ToList()
                : userCurrentRoom.RoomUsers.Select(x => x.Connection);

            var recipients = connectionsList.Except(except ?? new List<NetConnection>()).ToList();

            if (!recipients.Any()) { return; }

            var outgoingMessage = this.server.CreateMessage();

            outgoingMessage.Write(data);

            this.server.SendMessage(outgoingMessage, recipients, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}