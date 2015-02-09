namespace lilMess.Server.Network.Impl
{
    using System.Threading;
    using System.Threading.Tasks;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Requests;
    using lilMess.Server.Network.Services;

    public class ServerNetwork : INetwork
    {
        private readonly IService service;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private GotMessage gotMessage;

        public ServerNetwork(IService service)
        {
            this.service = service;
        }
        
        public string StartupServer(GotMessage gotMessageDelegate)
        {
            this.gotMessage = gotMessageDelegate;
            Task.Run(() => this.service.StartupServer(this.ProcessMessage), this.cts.Token);
            return string.Format("Прослушиваем порт {0}", 9997);
        }

        public void ShutdownServer()
        {
            this.cts.Cancel();
            this.service.ShutdownServer();
        }

        private void ProcessMessage(NetIncomingMessage incomingMessage, UserModel user)
        {
            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.Data:
                case NetIncomingMessageType.ConnectionApproval:
                    {
                        var packet = Serializer<Packet>.DeserializeObject(incomingMessage.PeekDataBuffer());

                        var request = new Request
                                          {
                                              IncomingMessage = incomingMessage,
                                              UserModel = user,
                                              Body = packet.PacketBody
                                          };

                        this.gotMessage(this.service.InvokeMethod(packet, request));
                        break;
                    }

                case NetIncomingMessageType.StatusChanged:
                    {
                        this.gotMessage(this.service.StatusChanged(user, incomingMessage));
                        break;
                    }

                default:
                    {
                        this.gotMessage(incomingMessage.ReadString());
                        break;
                    }
            }
        }
    }
}