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
        private int port;

        private RoleModel userRole;

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
            get { return connection; }
            set
            {
                connection = value;
                Port = Connection.RemoteEndPoint.Port;
            }
        }

        public RoleModel UserRole
        {
            get { return userRole; }
            set { userRole = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}