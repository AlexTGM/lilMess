namespace lilMess.Server.Network.Services
{
    using Lidgren.Network;

    using lilMess.Misc.Model;

    public interface IMessageProcessor
    {
        string ConnectionApproval(NetIncomingMessage incomingMessage, string guid, string login);

        string GetChatMessage(UserModel user, string message);

        string GetVoiceMessage(UserModel user, byte[] message);
    }
}