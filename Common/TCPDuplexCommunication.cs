using NAudio.Wave;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class TCPDuplexCommunication : BaseDuplexCommunication
    {
        private NetworkStream stream;
        private TcpClient client;

        private Thread thread;

        public TCPDuplexCommunication(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }

        public override void StartListening()
        {
            thread = new Thread(Listen);
            thread.Start(this);
        }

        private static void Listen(object state)
        {
            TCPDuplexCommunication comm = (TCPDuplexCommunication)state;
            int intSize = sizeof(int);
            while (true)
            {
                byte[] lengthBuffer = new byte[intSize];
                int recv = comm.stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                if (recv == intSize)
                {
                    int messageLen = BitConverter.ToInt32(lengthBuffer, 0);
                    byte[] messageBuffer = new byte[messageLen];
                    recv = comm.stream.Read(messageBuffer, 0, messageBuffer.Length);
                    if (recv == messageLen)
                    {
                        comm.OnMessageReceived(messageBuffer);
                    }
                }
            }
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
    }
}
