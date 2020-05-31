using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        public static List<ClientCommunication> clients = new List<ClientCommunication>();
        public static List<Channel> channels = new List<Channel>();

        static void Main(string[] args)
        {
            listen();
            channels.Add(new Channel("principal"));
            channels.Add(new Channel("jogando"));
            Console.ReadKey();
        }

        public static async Task listen()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                server.Start();

                while (true)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    clients.Add(new ClientCommunication(client));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
