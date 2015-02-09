namespace lilMess.Misc.Model
{
    using System;

    using Lidgren.Network;

    [Serializable]
    public class UserModel
    {
        [NonSerialized]
        private NetConnection connection;

        private string name;

        [NonSerialized]
        private string guid;

        public UserModel()
        {
        }

        public UserModel(NetConnection connection)
        {
            this.connection = connection;
        }
        
        public UserModel(NetConnection connection, string name)
        {
            this.connection = connection;
            this.name = name;
        }

        public NetConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }
    }
}