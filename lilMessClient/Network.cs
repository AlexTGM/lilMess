using Lidgren.Network;
using lilMessMisc;
using System;

namespace lilMessClient
{
    static class Network
    {
        public delegate void RecieveMessage(string message);
        private static RecieveMessage _recieve;

        private static NetClient _client;
        public static bool Connected
        { get { return _client.ConnectionStatus == NetConnectionStatus.Connected; } }

        public static void Initialise(RecieveMessage func)
        {
            _recieve = func;

            if (_client != null) return;

            var config = new NetPeerConfiguration("lilMess") 
                {AutoFlushSendQueue = false};

            _client = new NetClient(config);
            _client.RegisterReceivedCallback(GotMessage);
        }

        public static bool Connect(string ip, int port)
        {
            var started = DateTime.Now;

            _client.Start();

            var outMesssage = _client.CreateMessage();
            outMesssage.Write((byte)PacketType.LogIn);
            outMesssage.Write("test user");

            _client.Connect(ip, port, outMesssage);

            while ((DateTime.Now - started).Seconds < 10)
            {
                var im = _client.ReadMessage();

                if (im != null)
                {
                    return Connected;
                }
            }

            return false;
        }

        public static void GotMessage(object peer)
        {
            NetIncomingMessage im;

            while ((im = _client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        var text = im.ReadString();
                        _recieve(text);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)im.ReadByte();
                        var reason = im.ReadString();
                        _recieve(status + ": " + reason);
                        break;
                    case NetIncomingMessageType.Data:

                        var messageType = im.ReadByte();

                        switch (messageType)
                        {
                            case (byte)PacketType.ChatMessage:

                                var chat = im.ReadString();
                                _recieve(chat);

                                break;

                            case (byte)PacketType.VoiceMessage:

                                var voice = im.ReadBytes(im.LengthBytes - 1);

                                break;

                            case (byte)PacketType.ServerMessage:

                                var test = im.ReadInt32();

                                break;
                        }

                        break;

                    default:
                        _recieve("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
            }
        }

        public static void Send(string message)
        {
            var om = _client.CreateMessage();
            om.Write(Convert.ToByte(PacketType.ChatMessage));
            om.Write(message);
            _client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            _client.FlushSendQueue();
        }

        public static void Send(byte[] message)
        {
            var om = _client.CreateMessage();
            om.Write(Convert.ToByte(PacketType.VoiceMessage));
            om.Write(message);
            _client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            _client.FlushSendQueue();
        }

        public static void Shutdown()
        {
            _client.Disconnect("Requested by user");
        }
    }
}