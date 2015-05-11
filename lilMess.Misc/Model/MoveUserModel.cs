namespace lilMess.Misc.Model
{
    using System;

    [Serializable]
    public class MoveUserModel
    {
        public UserModel User { get; set; }

        public RoomModel Room { get; set; }
    }
}