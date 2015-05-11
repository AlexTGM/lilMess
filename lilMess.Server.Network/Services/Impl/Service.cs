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
        private readonly IRoomService _roomService;

        private readonly NetServer _server;

        private readonly Dictionary<PacketType, Func<Request, string>> _methods;

        public Service(IRoomService roomService, NetServer server, IMessageProcessor messageProcessor)
        {
            _roomService = roomService;
            _server = server;

            _methods = new Dictionary<PacketType, Func<Request, string>>
                           {
                               { messageProcessor.Chat.Key, messageProcessor.Chat.Value },
                               { messageProcessor.Audio.Key, messageProcessor.Audio.Value },
                               {
                                   messageProcessor.Connection.Key,
                                   messageProcessor.Connection.Value
                               },
                               { messageProcessor.Move.Key, messageProcessor.Move.Value }
                           };

            messageProcessor.SendNewPacket += SendPacket;
        }

        public void StartupServer(ProcessNewMessage processNewMessageDelegate)
        {
            _server.Start();

            while (_server.Status == NetPeerStatus.Running)
            {
                var message = _server.ReadMessage();

                if (message != null)
                {
                    processNewMessageDelegate.Invoke(message, _roomService.FindUser(message.SenderConnection));
                }

                Thread.Sleep(100);
            }
        }

        public string InvokeMethod(Packet packet, Request request)
        {
            return _methods[(PacketType)packet.PacketType].Invoke(request);
        }

        public void ShutdownServer()
        {
            _server.Shutdown("Сервер отключен администратором");
        }

        public string StatusChanged(UserModel user, NetIncomingMessage incomingMessage)
        {
            var status = (NetConnectionStatus)incomingMessage.ReadByte();
            if (status == NetConnectionStatus.Disconnected && user != null)
            {
                _roomService.RemoveUser(user.Id);
            }

            var serverInfo = new ServerInfoPacket(new ServerInfoBody { ServerRooms = _roomService.RoomList });

            SendPacket(Serializer<ChatMessagePacket>.SerializeObject(serverInfo));

            return string.Format("Пользователь {0} теперь имеет статус {1}", incomingMessage.SenderConnection, status);
        }

        private void SendPacket(byte[] data, UserModel sender = null, List<NetConnection> except = null)
        {
            if (!_server.Connections.Any()) return;

            var outgoingMessage = _server.CreateMessage();
            outgoingMessage.Write(data);

            if (sender == null)
            {
                _server.SendMessage(outgoingMessage, _server.Connections.ToList(), NetDeliveryMethod.ReliableOrdered, 0);
                return;
            }

            var userCurrentRoom = _roomService.GetUserCurrentRoom(sender.Id);

            var connectionsList = userCurrentRoom == null
                                      ? _server.Connections.ToList()
                                      : userCurrentRoom.RoomUsers.Select(x => x.Connection);

            var recipients = connectionsList.Except(except ?? new List<NetConnection>()).ToList();
            if (!recipients.Any())
            {
                return;
            }

            _server.SendMessage(outgoingMessage, recipients, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}