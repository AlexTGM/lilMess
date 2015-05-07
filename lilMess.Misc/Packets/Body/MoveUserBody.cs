namespace lilMess.Misc.Packets.Body
{
    using System;

    using lilMess.Misc.Model;

    [Serializable]
    public class MoveUserBody : Body
    {
        public MoveUserModel MoveUserModel { get; set; } 
    }
}