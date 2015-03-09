namespace lilMess.Client.Network
{
    using System.Collections.Generic;

    using lilMess.Misc.Model;

    public delegate void RecieveMessage(ChatMessageModel message);

    public delegate void ReciveAudio(byte[] message);

    public delegate void RefreshRoomList(List<RoomModel> rooms);

    public interface INetwork
    {
        int Port { get; }

        RecieveMessage Chat { get; set; }

        ReciveAudio Audio { get; set; }

        RefreshRoomList Refresh { get; set; }

        void Connect(string ip, int port, string login);

        void Shutdown();

        void SendChatMessage(string message);

        void SendVoiceMessage(byte[] message);
    }
}