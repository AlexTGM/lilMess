namespace lilMess.Client.Network
{
    using System.Collections.Generic;

    using lilMess.Misc.Model;

    public delegate void RecieveMessage(string message);

    public delegate void ReciveAudio(byte[] message);

    public delegate void RefreshRoomList(List<RoomModel> rooms); 

    public interface INetwork
    {
        string ServerInfo { get; }

        void StartClient();

        void Initialise(RecieveMessage func1, RefreshRoomList func2 = null, ReciveAudio func3 = null);

        void Connect(string ip, int port, string login);

        void Shutdown();

        void SendChatMessage(string message);

        void SendVoiceMessage(byte[] message);
    }
}