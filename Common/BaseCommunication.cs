using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class Request : System.Attribute
    {
        public string Value { get; }

        public Request(string value)
        {
            this.Value = value;
        }
    }

    public abstract class BaseCommunication
    {
        protected UDPDuplexCommunication audioSocket;
        protected TCPDuplexCommunication textSocket;
        private readonly Dictionary<string, Action<Message, RespondMessage>> messageHandlers = new Dictionary<string, Action<Message, RespondMessage>>();
        private readonly Dictionary<string, Action<Message>> notificationHandlers = new Dictionary<string, Action<Message>>();
        private readonly ConcurrentDictionary<Guid, Message> responses = new ConcurrentDictionary<Guid, Message>();

        public delegate void RespondMessage(Message message);
        public TCPDuplexCommunication TextSocket { get => textSocket; set { SetTextSocket(value); } }
        public UDPDuplexCommunication AudioSocket { get => audioSocket; set { SetAudioSocket(value); } }
        public bool TextConnected { get { return (bool)(textSocket?.Connected); } }
        public bool AudioConnected { get { return (bool)(audioSocket?.Connected); } }

        public BaseCommunication(TcpClient tcpClient)
        {
            textSocket = new TCPDuplexCommunication(tcpClient);
            TextSocket = textSocket;
        }

        public BaseCommunication(TCPDuplexCommunication tcpClient)
        {
            TextSocket = tcpClient;
        }

        public BaseCommunication()
        { }

        public void ConnectTextServer(IPEndPoint endPoint)
        {
            TCPDuplexCommunication textSocket = new TCPDuplexCommunication();
            textSocket.Connect(endPoint);
            TextSocket = textSocket;
        }

        public void ConnectAudioServer(IPEndPoint endPoint)
        {
            UDPDuplexCommunication audioSocket = new UDPDuplexCommunication();
            audioSocket.Connect(endPoint);
            AudioSocket = audioSocket;
        }

        public void Disconnect()
        {
            if (AudioConnected)
                audioSocket.Disconnect();

            if (TextConnected)
                textSocket.Disconnect();
        }

        public void SetTextSocket(TCPDuplexCommunication textSocket)
        {
            this.textSocket = textSocket;
            textSocket.MessageReceived += OnMessageReceived;
            textSocket.StartListening();
        }

        public void SetAudioSocket(UDPDuplexCommunication audioSocket)
        {
            this.audioSocket = audioSocket;
            audioSocket.MessageReceived += OnAudioDataReceived;
            audioSocket.StartListening();
        }

        private void OnMessageReceived(byte[] message)
        {
            Message m = Message.Convert(message);

            if (m.IsResponse())
            {
                Guid requestId = m.RequestId;
                responses.TryAdd(requestId, m);
                Timer t = null;
                t = new Timer(
                    (state) =>
                    {
                        Message _;
                        responses.TryRemove((Guid)state, out _);
                        _ = null;
                        t.Dispose();
                    },
                    requestId,
                    60000,
                    Timeout.Infinite
                );
            }
            else if(m.IsRequest())
            {
                Guid requestId = m.RequestId;
                Action<Message, RespondMessage> handler;
                RespondMessage respondMessage = (mss) => { Respond(mss, requestId); };
                if (messageHandlers.TryGetValue(m.Request, out handler))
                    handler(m, respondMessage);
                else
                    DefaultMessageHandler(respondMessage);
            }else if (m.IsNotification())
            {
                Action<Message> handler;
                if (notificationHandlers.TryGetValue(m.Request, out handler))
                    handler(m);
            }
        }

        protected void DefaultMessageHandler(RespondMessage respondMessage)
        {
            Message m = Message.Create404Response();
            m.message = "What you are requesting is unknown by the server.";
            respondMessage(m);
        }

        protected void RegisterMessageHandler(Action<Message, RespondMessage> handler)
        {
            Request request = (Request)handler.Method.GetCustomAttributes(typeof(Request), false)[0];
            messageHandlers.Add(request.Value, handler);
        }

        protected void RegisterNotificationHandler(Action<Message> handler)
        {
            Request request = (Request)handler.Method.GetCustomAttributes(typeof(Request), false)[0];
            notificationHandlers.Add(request.Value, handler);
        }

        protected abstract void OnAudioDataReceived(byte[] message);

        public Message Request(Message message)
        {
            Guid guid = Guid.NewGuid();
            message.SetAsRequest(guid);
            SendMessage(Message.Convert(message));
            return WaitResponse(guid);
        }

        public Task NotifyAsync(Message message)
        {
            return Task.Run(() =>
            {
                Notify(message);
            });
        }

        public void Notify(Message message)
        {
            message.SetAsNotification();
            SendMessage(Message.Convert(message));
        }

        private Message WaitResponse(Guid requestId)
        {
            Message m;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (!responses.TryRemove(requestId, out m))
            {
                if (stopWatch.Elapsed.TotalSeconds >= 60)
                {
                    stopWatch.Stop();
                    m = Message.CreateTimeoutResponse();
                    m.SetRequestId(requestId);
                    break;
                }
            }
            return m;
        }

        private void Respond(Message message, Guid requestId)
        {
            message.SetAsResponse(requestId);
            SendMessage(Message.Convert(message));
        }

        private void SendMessage(byte[] message)
        {
            textSocket?.WriteMessage(message);
        }

        public virtual void SendAudioData(byte[] data)
        {
            audioSocket?.WriteMessage(data);
        }
    }
}
