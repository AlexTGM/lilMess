using Lidgren.Network;
using System.Threading;
using System.Windows;

namespace lilMessClient
{
    static class Network
    {
        public delegate void RecieveMessage(string message);
        private static RecieveMessage recieve;

        private static NetClient s_client;
        public static NetConnectionStatus CurrentStatus
        { get { return s_client.ConnectionStatus; } }

        public static void Initialise(RecieveMessage func)
        {
            recieve = func;

            var config = new NetPeerConfiguration("lilMess");
            config.AutoFlushSendQueue = false;
            s_client = new NetClient(config);

            s_client.RegisterReceivedCallback(new SendOrPostCallback(GotMessage)); 
        }

        public static void Connect(string host, int port)
        {
            s_client.Start();
            NetOutgoingMessage hail = s_client.CreateMessage("This is the hail message");
            s_client.Connect(host, port, hail);
        }

        public static void GotMessage(object peer)
        {
            NetIncomingMessage im;
            while ((im = s_client.ReadMessage()) != null)
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
                        recieve(status.ToString() + ": " + reason);

                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();
                        recieve(chat);
                        break;
                    default:
                        recieve("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
            }
        }

        public static void Send(string text)
        {
            NetOutgoingMessage om = s_client.CreateMessage(text);
            s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            s_client.FlushSendQueue();
        }

        public static void Shutdown()
        {
            s_client.Disconnect("Requested by user");
        }
    }
}