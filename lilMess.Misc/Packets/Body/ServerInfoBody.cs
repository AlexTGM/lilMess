namespace lilMess.Misc.Packets.Body
{
    using System;
    using System.Collections.Generic;

    using lilMess.Misc.Model;

    [Serializable]
    public class ServerInfoBody : Body
    {
        public List<RoomModel> Rooms { get; set; }
    }
}