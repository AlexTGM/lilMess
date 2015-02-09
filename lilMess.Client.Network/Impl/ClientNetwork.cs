namespace lilMess.Client.Network.Impl
{
    using System;
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets;
    using lilMess.Misc.Packets.Body;

    using Guid = lilMess.Tools.Guid;

    public class ClientNetwork : INetwork
    {
        private readonly List<RoomModel> roomsList = new List<RoomModel>();

        private RecieveMessage chat;

        private ReciveAudio audio;

        private RefreshRoomList refresh;

        private NetClient client;

        private string serverInfo = "Disconnected";

        public string ServerInfo
        {
            get { return this.serverInfo; }
        }

        public void StartClient()
        {
            this.client = new NetClient(new NetPeerConfiguration("lilMess"));

            this.client.RegisterReceivedCallback(
                peer =>
                    {
                        NetIncomingMessage incomingMessage;

                        while ((incomingMessage = client.ReadMessage()) != null)
                        {
                            ReadMessage(incomingMessage);
                        }
                    });

            this.client.Start();
        }

        public void Initialise(RecieveMessage func1, RefreshRoomList func2 = null, ReciveAudio func3 = null)
        {
            this.chat = func1;
            this.refresh = func2;
            this.audio = func3;

            this.StartClient();
        }

        public void Connect(string ip, int port, string login)
        {
            this.serverInfo = string.Format("{0}:{1}", ip, port);

            var started = DateTime.Now;

            var authenticationPacketBody = new AuthenticationBody { Login = login, Guid = Guid.GetUniqueHardwareId() };
            var auth = new AuthenticationPacket(authenticationPacketBody);

            var sendBuffer = new NetBuffer();
            sendBuffer.Write(Serializer<AuthenticationPacket>.SerializeObject(auth));

            var outMesssage = this.client.CreateMessage();
            outMesssage.Write(sendBuffer);

            this.client.Connect(ip, port, outMesssage);

            while ((DateTime.Now - started).Seconds < 5)
            {
                var im = this.client.ReadMessage();

                if (im != null && im.MessageType == NetIncomingMessageType.StatusChanged)
                {
                    if (this.client.ConnectionStatus == NetConnectionStatus.Connected)
                    {
                        return;
                    }

                    throw new Exception("Wrong creditianals!");
                }
            }

            throw new Exception("Can't establish a connection to the server!");
        }

        public void Shutdown()
        {
            this.client.Disconnect("Requested by user");
        }

        public void SendChatMessage(string message)
        {
            var chatMessage = new ChatMessagePacket(new ChatMessageBody { Message = message });

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(chatMessage));
        }

        public void SendVoiceMessage(byte[] message)
        {
            var voiceMessage = new VoiceMessagePacket(new VoiceMessageBody { Message = message });

            this.SendPacket(Serializer<VoiceMessagePacket>.SerializeObject(voiceMessage));
        }

        private void SendPacket(byte[] data)
        {
            var outgoingMessage = this.client.CreateMessage();

            var sendBuffer = new NetBuffer();
            sendBuffer.Write(data);

            outgoingMessage.Write(sendBuffer);

            this.client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        private void AddNewRoomRange(IEnumerable<RoomModel> rooms)
        {
            this.roomsList.Clear();
            this.roomsList.AddRange(rooms);
            this.refresh(this.roomsList);
        }

        private void ReadMessage(NetIncomingMessage incomingMessage)
        {
            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    if (this.chat != null) this.chat(incomingMessage.ReadString());
                    break;
                case NetIncomingMessageType.StatusChanged:
                    var status = (NetConnectionStatus)incomingMessage.ReadByte();
                    string reason = incomingMessage.ReadString();
                    if (this.chat != null) this.chat(status + ": " + reason);
                    break;
                case NetIncomingMessageType.Data:

                    var packet = Serializer<Packet>.DeserializeObject(incomingMessage.PeekDataBuffer());

                    switch (packet.PacketType)
                    {
                        case (byte)PacketType.ChatMessage:

                            var message = string.Format(
                                "{0}: {1}",
                                ((ChatMessageBody)packet.PacketBody).Sender,
                                ((ChatMessageBody)packet.PacketBody).Message);

                            this.chat(message);
                            break;

                        case (byte)PacketType.VoiceMessage:

                            this.audio(((VoiceMessageBody)packet.PacketBody).Message);
                            break;

                        case (byte)PacketType.ServerMessage:

                            var rooms = ((ServerInfoBody)packet.PacketBody).Rooms;
                            this.AddNewRoomRange(rooms.ToArray());
                            break;
                    }

                    break;

                default:
                    this.chat(
                        string.Format(
                            "Unhandled type: {0} {1} bytes",
                            incomingMessage.MessageType,
                            incomingMessage.LengthBytes));
                    break;
            }
        }
    }
}