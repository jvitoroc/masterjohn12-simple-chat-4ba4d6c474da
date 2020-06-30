using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static List<Client> clients = new List<Client>();
        public static List<Channel> channels = new List<Channel>();
        public static Dictionary<string, Channel> channelsIdMap = new Dictionary<string, Channel>();
        public static TCPDuplexListener tcpDuplexListener = new TCPDuplexListener();
        public static UDPDuplexListener udpDuplexListener = new UDPDuplexListener();

        static void Main(string[] args)
        {
            tcpDuplexListener.StartListening(TCPDuplexCommunicationAccepted);
            udpDuplexListener.StartListening(UDPDuplexCommunicationAccepted);

            Console.ReadKey();
        }

        public static void TCPDuplexCommunicationAccepted(TCPDuplexCommunication tcpClient)
        {
            clients.Add(new Client(tcpClient));
        }

        public static void UDPDuplexCommunicationAccepted(UDPDuplexCommunication udpClient)
        {
            Client client = clients.Find((c) => c.TextSocket.RemoteIPEndPoint.Address.Equals(udpClient.RemoteIPEndPoint.Address));
            if (client != null && client.AudioSocket == null)
            {
                client.AudioSocket = udpClient;
            }
        }

        public static Channel CreateChannel()
        {
            lock (channels)
            {
                Channel c = new Channel();
                channels.Add(c);
                channelsIdMap.Add(c.Id, c);
                Timer t = null;
                t = new Timer((state) =>
                {
                    Channel channel = (Channel)state;
                    if (channel.ClientCount == 0)
                    {
                        Program.DeleteChannel(c);
                    }
                    t.Dispose();
                }, c, 60000, 0);
                return c;
            }
        }

        public static void DeleteChannel(Channel channel)
        {
            lock (channels)
            {
                if (channels.Remove(channel))
                {
                    channelsIdMap.Remove(channel.Id);
                }
            }
        }

        public static void JoinChannel(Client client, string channelId)
        {
            Channel c;
            if(channelsIdMap.TryGetValue(channelId, out c))
            {
                c.AddClient(client);
            }
        }
    }
}
