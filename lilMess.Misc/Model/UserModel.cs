﻿namespace lilMess.Misc.Model
{
    using Lidgren.Network;

    using Newtonsoft.Json;

    public class UserModel
    {
        [JsonIgnore]
        private NetConnection _connection;

        [JsonIgnore]
        private string _guid;

        private string _userName;

        private int _port;

        private RoleModel _userRole;

        public UserModel() : this(null)
        {
        }

        public UserModel(NetConnection connection)
        {
            _connection = connection;
        }

        public UserModel(NetConnection connection, string userName)
            : this(connection)
        {
            _userName = userName;
        }

        public string Id { get; set; }

        [JsonIgnore]
        public NetConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
                Port = Connection.RemoteEndPoint.Port;
            }
        }

        public RoleModel UserRole
        {
            get
            {
                return _userRole;
            }
            set
            {
                _userRole = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }
    }
}