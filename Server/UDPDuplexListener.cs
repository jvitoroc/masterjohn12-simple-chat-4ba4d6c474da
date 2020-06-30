using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UDP;
using Common;

namespace Server
{
    public class UDPDuplexListener
    {
        private Thread listeningThread;
        private Action<UDPDuplexCommunication> clientAccepted;
        private Action started;
        private ConcurrentDictionary<string, UDPDuplexCommunication> clients;
        private UdpClient udpClient;

        public void StartListening(Action<UDPDuplexCommunication> clientAccepted, Action started = null)
        {
            this.clientAccepted = clientAccepted;
            this.started = started;
            this.clients = new ConcurrentDictionary<string, UDPDuplexCommunication>();
            this.udpClient = new UdpClient();

            this.udpClient.Client.ReceiveBufferSize = 1024;

            listeningThread = new Thread(Listening);
            listeningThread.Start(this);
        }

        private static void Listening(object state)
        {
            UDPDuplexListener listener = (UDPDuplexListener)state;
            UDPListener udpListener = new UDPListener(new IPEndPoint(IPAddress.Loopback, 13001), 2);

            try
            {
                udpListener.Start();
                if (listener.started != null)
                    listener.started();
                Console.WriteLine("Servidor de áudio iniciado na porta " + udpListener.LocalIPEndPoint.Port.ToString());
                while (true)
                {
                    UDPClient client = udpListener.AcceptClient();
                    listener.clientAccepted(new UDPDuplexCommunication(client));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                udpListener.Stop();
            }
        }
    }
}
