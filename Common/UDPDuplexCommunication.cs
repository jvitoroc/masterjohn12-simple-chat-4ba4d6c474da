using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{    
    public class UDPDuplexCommunication : BaseDuplexCommunication
    {
        private UdpClient client;

        private Thread thread;

        public UDPDuplexCommunication(UdpClient client)
        {
            this.client = client;
        }
        public override void StartListening()
        {
            thread = new Thread(Listen);
            thread.Start(this);
        }

        private static void Listen(object state)
        {
            UDPDuplexCommunication comm = (UDPDuplexCommunication)state;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] data = comm.client.Receive(ref ipEndPoint);
                comm.OnMessageReceived(data);
            }
        }

        protected override void OnMessageReceived(byte[] message){
            base.OnMessageReceived(message);
        }

        public override void WriteMessage(byte[] message)
        {
            client.Send(message, message.Length);
        }
    }
}
