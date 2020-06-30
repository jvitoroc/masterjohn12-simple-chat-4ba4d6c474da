using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Common
{
    public class TCPDuplexCommunication : BaseDuplexCommunication
    {
        private NetworkStream stream;
        private TcpClient tcpClient;

        public IPEndPoint RemoteIPEndPoint { get => (IPEndPoint)tcpClient.Client.RemoteEndPoint; }
        public override bool Connected { get { return (bool)(tcpClient?.Connected); } }

        public TCPDuplexCommunication(TcpClient client)
        {
            this.tcpClient = client;
            this.stream = client.GetStream();
        }

        public TCPDuplexCommunication()
        {}

        public void Connect(IPEndPoint endPoint)
        {
            this.tcpClient = new TcpClient();
            this.tcpClient.Connect(endPoint);
            this.stream = this.tcpClient.GetStream();
        }

        public override void Disconnect()
        {
            if (Connected)
            {
                tcpClient.Close();
            }
        }

        public override void StartListening()
        {
            int intSize = sizeof(int);
            int packetSize;
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[intSize];
                    this.stream.Read(buffer, 0, intSize);
                    packetSize = BitConverter.ToInt32(buffer, 0);
                    buffer = new byte[packetSize];
                    this.stream.Read(buffer, 0, packetSize);
                    Task.Run(() => { OnMessageReceived(buffer); });
                }
            });
        }

        protected override void OnMessageReceived(byte[] message)
        {
            base.OnMessageReceived(message);
        }

        public override void WriteMessage(byte[] message)
        {
            int intSize = sizeof(int);
            byte[] messageLength = BitConverter.GetBytes(message.Length);
            byte[] data = new byte[intSize + message.Length];

            for (int i = 0; i < sizeof(int); i++)
            {
                data[i] = messageLength[i];
            }

            for (int i = 0; i < message.Length; i++)
            {
                data[i + intSize] = message[i];
            }

            stream.Write(data, 0, data.Length);
        }

        private Task WriteMessageAsync(byte[] message)
        {
            return Task.Run(() => WriteMessage(message));
        }
    }

}
