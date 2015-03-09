namespace lilMess.Client.Network.Impl
{
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets;
    using lilMess.Misc.Packets.Body;

    using Guid = lilMess.Tools.HardwareGuid;

    public class ClientNetwork : INetwork
    {
        private readonly List<RoomModel> roomsList = new List<RoomModel>();

        private readonly NetClient client;

        public ClientNetwork(NetClient client)
        {
            this.client = client;
            this.client.RegisterReceivedCallback(this.Callback);
        }

        public RecieveMessage Chat { get; set; }

        public ReciveAudio Audio { get; set; }

        public RefreshRoomList Refresh { get; set; }

        public int Port { get { return this.client.Port; } }

        public void Connect(string ip, int port, string login)
        {
            if (this.client.ConnectionStatus == NetConnectionStatus.Connected) { this.Shutdown(); }

            this.client.Start();

            var authenticationPacketBody = new AuthenticationBody { Login = login, Guid = Guid.GetUniqueHardwareId() };
            var auth = new AuthenticationPacket(authenticationPacketBody);

            var sendBuffer = new NetBuffer();
            sendBuffer.Write(Serializer<AuthenticationPacket>.SerializeObject(auth));

            var outMesssage = this.client.CreateMessage();
            outMesssage.Write(sendBuffer);

            this.client.Connect(ip, port, outMesssage);
        }

        public void Shutdown()
        {
            this.client.Disconnect("Requested by user");
        }

        public void SendChatMessage(string message)
        {
            var chatMessage = new ChatMessagePacket(new ChatMessageBody { ChatMessageModel = new ChatMessageModel { MessageContent = message } });

            this.SendPacket(Serializer<ChatMessagePacket>.SerializeObject(chatMessage));
        }

        public void SendVoiceMessage(byte[] message)
        {
            var voiceMessage = new VoiceMessagePacket(new VoiceMessageBody { Message = message });

            this.SendPacket(Serializer<VoiceMessagePacket>.SerializeObject(voiceMessage));
        }

        private void Callback(object peer)
        {
            NetIncomingMessage incomingMessage;

            while ((incomingMessage = this.client.ReadMessage()) != null) { this.ReadMessage(incomingMessage); }
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
            var refresh = this.Refresh;

            this.roomsList.Clear();
            this.roomsList.AddRange(rooms);

            if (refresh != null) refresh(this.roomsList);
        }

        private void ReadMessage(NetIncomingMessage incomingMessage)
        {
            var chat = this.Chat;
            var audio = this.Audio;

            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    if (chat != null) { chat(this.SystemMessage(incomingMessage.ReadString())); }

                    break;
                case NetIncomingMessageType.StatusChanged:
                    {
                        var status = (NetConnectionStatus)incomingMessage.ReadByte();
                        var reason = incomingMessage.ReadString();
                        if (chat != null) { chat(this.SystemMessage(string.Format("{0} : {1}", status, reason))); }

                        break;
                    }
                case NetIncomingMessageType.Data:

                    var packet = Serializer<Packet>.DeserializeObject(incomingMessage.PeekDataBuffer());

                    switch (packet.PacketType)
                    {
                        case (byte)PacketType.ChatMessage:

                            chat(((ChatMessageBody)packet.PacketBody).ChatMessageModel);

                            break;

                        case (byte)PacketType.VoiceMessage:

                            audio(((VoiceMessageBody)packet.PacketBody).Message);
                            break;

                        case (byte)PacketType.ServerMessage:

                            var rooms = ((ServerInfoBody)packet.PacketBody).ServerRooms;
                            this.AddNewRoomRange(rooms.ToArray());
                            break;
                    }

                    break;

                default:
                    {
                        if (chat != null) { chat(this.SystemMessage(string.Format("Unhandled type: {0}", incomingMessage.MessageType))); }

                        break;
                    }
            }
        }

        private ChatMessageModel SystemMessage(string message)
        {
            var system = new UserModel { UserName = "system", UserRole = new RoleModel { RoleColor = "Yellow", RoleName = "system" } };

            return new ChatMessageModel { MessageContent = message, MessageSender = system };
        }
    }
}