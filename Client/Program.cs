using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common;

namespace Client
{
    class Program
    {

        public static ServerCommunication server;
        public static string name;
        public static string messages = "";
        public static string buffer = "";

        static void Main(string[] args)
        {   
            name = Console.ReadLine();
            try
            {
                Int32 port = 13000;
                TcpClient _server = new TcpClient("localhost", port);
                server = new ServerCommunication(_server);
                //server.UseAudioDriver(new AudioDriver());
                Task.Run(() => RenderScreen());


                while (true)
                {
                    Console.ReadKey();
                }
                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static async Task RenderScreen()
        {
            TextReader tr = Console.In;
            while (true)
            {
                Console.Clear();
                Console.Write(messages);
                Console.Write("\n>> ");
                
                buffer = await tr.ReadLineAsync();
     
                Message m = new Message();
                m.type = PacketType.Text;
                m.message = buffer;
                m.from = name;

                buffer = "";

                //server.WriteMessage(m);
            }
        }
    }
}
