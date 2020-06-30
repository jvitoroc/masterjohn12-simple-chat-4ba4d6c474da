using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;

namespace Server
{
    public class TCPDuplexListener
    {
        private Thread listeningThread;
        private Action<TCPDuplexCommunication> clientAccepted;
        private Action started;

        public void StartListening(Action<TCPDuplexCommunication> clientAccepted, Action started = null)
        {
            this.clientAccepted = clientAccepted;
            this.started = started;

            listeningThread = new Thread(Listening);
            listeningThread.Start(this);
        }

        private static void Listening(object state)
        {
            TCPDuplexListener listener = (TCPDuplexListener)state;
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 13000);

            try
            {
                tcpListener.Start();
                if(listener.started != null)
                    listener.started();
                Console.WriteLine("Servidor de texto iniciado na porta " + ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString());
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    listener.clientAccepted(new TCPDuplexCommunication(client));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                tcpListener.Stop();
            }
        }
    }
}
