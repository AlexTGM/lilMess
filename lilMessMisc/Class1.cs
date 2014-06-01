namespace lilMessMisc
{
    public enum PacketType : byte
    {
        LogIn,
        LogOut,
        ChatMessage,
        VoiceMessage,
        ServerMessage,
        None
    }
}