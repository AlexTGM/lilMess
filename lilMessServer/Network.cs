using Lidgren.Network;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace lilMessServer
{
    class Network
    {
        private static NetServer s_server;
        public delegate void RecieveMessage(string message);
        private static RecieveMessage recieve;

        public static void Initialise(RecieveMessage func)
        {
            recieve = func;

            var config = new NetPeerConfiguration("lilMess");
			config.MaximumConnections = 100;
			config.Port = 9997;
			s_server = new NetServer(config);
        }

        public static void StartServer()
        {
            s_server.Start();

            Thread thread = new Thread(Idle);
            thread.Start();
        }

        private static void Idle()
        {
            while (s_server.Status == NetPeerStatus.Running)
            {
                NetIncomingMessage im;

                while ((im = s_server.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            string text = im.ReadString();
                            recieve(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                            string reason = im.ReadString();
                            im.SenderConnection.Disconnect("LALALL");
                            recieve(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                            // UpdateConnectionsList();
                            break;
                        case NetIncomingMessageType.Data:
                            // incoming chat message from a client
                            string chat = im.ReadString();

                            recieve("Broadcasting '" + chat + "'");

                            // broadcast this to all connections, except sender
                            List<NetConnection> all = s_server.Connections; // get copy
                            all.Remove(im.SenderConnection);

                            if (all.Count > 0)
                            {
                                NetOutgoingMessage om = s_server.CreateMessage();
                                om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                                s_server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            break;
                        default:
                            recieve("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }
                }

                Thread.Sleep(1);
            }
        }

        public static void Shutdown()
        {
            s_server.Shutdown("Requested by user");
        }
    }
}