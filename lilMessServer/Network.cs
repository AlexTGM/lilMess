using Lidgren.Network;
using lilMessMisc;
using System.Collections.Generic;
using System.Threading;

namespace lilMessServer
{
    class Network
    {
        private static Thread _listenThread;

        private static NetServer _server;

        public delegate void RecieveMessage(string message);
        private static RecieveMessage _recieve;

        public static void Initialise(RecieveMessage func)
        {
            _recieve = func;

            var config = new NetPeerConfiguration("lilMess") 
                { MaximumConnections = 100, Port = 9997 };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

			_server = new NetServer(config);
        }

        public static void StartServer()
        {
            _listenThread = new Thread(Listen);
            _listenThread.Start();
        }

        private static void Listen()
        {
            _server.Start();

            while (_server.Status == NetPeerStatus.Running)
            {
                NetIncomingMessage im;

                while ((im = _server.ReadMessage()) != null)
                {
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            if (im.ReadByte() == (byte)PacketType.LogIn)
                            {
                                im.SenderConnection.Approve();
                                var nick = im.ReadString(); 
                            }
                            break;
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            var text = im.ReadString();
                            _recieve(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus)im.ReadByte();

                            if (status == NetConnectionStatus.Connected)
                            {
                                NetOutgoingMessage om = _server.CreateMessage();
                                om.Write((byte)PacketType.ServerMessage);
                                om.Write(1);
                                _server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                            }

                            var reason = im.ReadString();
                            _recieve(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                            break;
                        case NetIncomingMessageType.Data:

                            var messageType = im.ReadByte();

                            switch (messageType)
                            {
                                case (byte)PacketType.ChatMessage:

                                    var chat = im.ReadString();

                                    _recieve("Broadcasting '" + chat + "'");

                                    if (_server.Connections.Count > 0)
                                    {
                                        var om = _server.CreateMessage();
                                        om.Write((byte)PacketType.ChatMessage);
                                        om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                                        _server.SendMessage(om, _server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    }

                                    break;

                                case (byte)PacketType.VoiceMessage:

                                    var voice = im.ReadBytes(im.LengthBytes - 1);

                                    var all = _server.Connections;
                                    all.Remove(im.SenderConnection);

                                    if (all.Count > 0)
                                    {
                                        var om = _server.CreateMessage();
                                        om.Write((byte)PacketType.VoiceMessage);
                                        om.Write(voice);
                                        _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                    }

                                    break;
                            }

                            break;

                        default:
                            _recieve("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }
                }

                Thread.Sleep(1);
            }
        }

        public static void Shutdown()
        {
            _server.Shutdown("Requested by user");
            _listenThread.Abort();
        }
    }
}