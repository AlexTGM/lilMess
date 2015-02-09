namespace lilMess.Misc.Packets.Body
{
    using System;

    [Serializable]
    public class AuthenticationBody : Body
    {
        public string Login { get; set; }

        public string Guid { get; set; }
    }
}