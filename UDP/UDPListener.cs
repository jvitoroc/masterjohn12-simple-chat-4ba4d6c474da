using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace UDP
{
    public class UDPListener
    {
        private const int ACCEPT_CLIENT_RESET_TIMEOUT = 5000;

        private UdpClient udpClient;
        private ConcurrentDictionary<string, UDPClient> clients;
        private ConcurrentQueue<UDPClient> queue;
        private CancellationTokenSource token;
        private readonly AutoResetEvent waitClientSignal = new AutoResetEvent(false);
        private bool listening;

        private uint workers;

        private Task[] listeningTasks;

        public IPEndPoint LocalIPEndPoint { get { return (IPEndPoint)udpClient.Client.LocalEndPoint; } }

        public UDPListener(IPEndPoint localIpEndPoint, uint workers = 1)
        {
            udpClient = new UdpClient();
            clients = new ConcurrentDictionary<string, UDPClient>();
            queue = new ConcurrentQueue<UDPClient>();

            listening = false;

            this.workers = workers;

            udpClient.Client.Bind(localIpEndPoint);
        }

        public void Start()
        {
            if (!listening)
            {
                StopTasks();

                listeningTasks = new Task[workers];
                token = new CancellationTokenSource();

                for (int i = 0; i < listeningTasks.Length; i++)
                {
                    listeningTasks[i] = Task.Factory.StartNew(() => { Listening(); }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }

                listening = true;
            }
        }

        public void Stop()
        {
            StopTasks();
            listening = false;
        }

        private void StopTasks()
        {
            if (listening)
            {
                if (!token.IsCancellationRequested)
                    token.Cancel();

                if (listeningTasks != null)
                    Task.WaitAll(listeningTasks);

                token.Dispose();

                listeningTasks = null;
            }
        }

        public UDPClient AcceptClient()
        {
            UDPClient client;
            
            while(!queue.TryDequeue(out client))
            {
                waitClientSignal.WaitOne(ACCEPT_CLIENT_RESET_TIMEOUT);
            }

            return client;
        }

        private void Listening()
        {
            token.Token.ThrowIfCancellationRequested();

            UDPClient client;
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            string clientKey;

            while (true)
            {
                if (token.Token.IsCancellationRequested)
                    token.Token.ThrowIfCancellationRequested();

                byte[] buffer = udpClient.Receive(ref remoteIpEndPoint);
                clientKey = remoteIpEndPoint.ToString();

                if (!this.clients.TryGetValue(clientKey, out client))
                {
                    client = new UDPClient(udpClient, remoteIpEndPoint, 10);
                    clients.TryAdd(clientKey, client);
                    queue.Enqueue(client);
                    waitClientSignal.Set();
                }

                client.WriteInternal(buffer);
            }
        }

    }
}
