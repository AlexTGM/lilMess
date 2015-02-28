namespace lilMess.Misc.Model
{
    using System;

    using Lidgren.Network;

    [Serializable]
    public class UserModel
    {
        [NonSerialized]
        private NetConnection connection;

        private string userName;

        [NonSerialized]
        private string guid;

        public UserModel() {}

        public UserModel(NetConnection connection)
        {
            this.connection = connection;
        }

        public UserModel(NetConnection connection, string userName)
            : this(connection)
        {
            this.userName = userName;
        }

        public NetConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public string Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }
    }
}