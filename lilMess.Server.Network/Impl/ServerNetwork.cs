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
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public ServerNetwork(IService service, IStatisticsService statisticsService)
        {
            Service = service;
            StatisticsService = statisticsService;
        }

        public IStatisticsService StatisticsService { get; private set; }

        public GotMessage GotMessage { get; set; }

        private IService Service { get; set; }
        
        public string StartupServer()
        {
            Task.Run(() => Service.StartupServer(ProcessMessage), cts.Token);

            return string.Format("Прослушиваем порт {0}", 9997);
        }

        public void ShutdownServer()
        {
            cts.Cancel();
            Service.ShutdownServer();
        }

        private void ProcessMessage(NetIncomingMessage incomingMessage, UserModel user)
        {
            var message = GotMessage;

            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.Data:
                case NetIncomingMessageType.ConnectionApproval:
                    var packet = Serializer<Packet>.DeserializeObject(incomingMessage.PeekDataBuffer());

                    var request = new Request { IncomingMessage = incomingMessage, UserModel = user, Body = packet.PacketBody };

                    if (message != null)
                    {
                        message(Service.InvokeMethod(packet, request));
                    }
                    break;

                case NetIncomingMessageType.StatusChanged:
                    if (message != null)
                    {
                        message(Service.StatusChanged(user, incomingMessage));
                    }
                    break;

                default:
                    if (message != null)
                    {
                        message(incomingMessage.ReadString());
                    }
                    break;
            }
        }
    }
}