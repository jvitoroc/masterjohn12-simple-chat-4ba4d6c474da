using System.Net;
using System.Threading.Tasks;
using UDP;

namespace Common
{
    public class UDPDuplexCommunication : BaseDuplexCommunication
    {
        private UDPClient udpClient;

        public IPEndPoint RemoteIPEndPoint { get => udpClient.RemoteIPEndPoint; }
        public override bool Connected { get { return (bool)(udpClient?.Connected); } }

        public UDPDuplexCommunication(UDPClient udpClient)
        {
            this.udpClient = udpClient;
        }
        
        public UDPDuplexCommunication()
        { }

        public void Connect(IPEndPoint endPoint)
        {
            this.udpClient = new UDPClient();
            this.udpClient.Connect(endPoint);
        }

        public override void Disconnect()
        {
            if (Connected)
            {
                udpClient.Close();
            }
        }

        public override void StartListening()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = this.udpClient.Receive();
                    OnMessageReceived(buffer);
                }
            });
        }

        protected override void OnMessageReceived(byte[] message)
        {
            base.OnMessageReceived(message);
        }

        public override void WriteMessage(byte[] message)
        {
            udpClient.Send(message, message.Length);
        }
    }
}
