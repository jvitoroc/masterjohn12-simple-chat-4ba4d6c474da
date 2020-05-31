using System;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Server
{
    public class ClientCommunication {
  
        private UDPDuplexCommunication audio;
        private TCPDuplexCommunication text;
        private Channel channel;

        private Guid id;

        public ClientCommunication(TcpClient client)
        {
            id = Guid.Empty;
            text = new TCPDuplexCommunication(client);
            text.MessageReceived += OnTextMessageReceived;
            text.StartListening();
        }

        public Channel Channel { get => channel; }

        private void OnTextMessageReceived(byte[] message)
        {
            //Message m = Message.Convert(message);
            channel?.BroadcastTextMessage(message);
        }

        private void OnAudioDataReceived(byte[] message)
        {
            channel?.BroadcastAudioData(message);
        }

        public void SendTextMessage(string message)
        {
            SendTextMessage(Encoding.UTF8.GetBytes(message));
        }

        public void SendTextMessage(byte[] message)
        {
            text?.WriteMessage(message);
        }

        public void SendAudioData(byte[] data)
        {
            audio?.WriteMessage(data);
        }

        public void JoinChannel(Channel channel)
        {
            channel.AddClient(this);
            this.channel = channel;
        }

        public void LeaveChannel()
        {
            if (channel != null || channel.HasClient(this))
            {
                channel.DisconnectClient(this);
                this.channel = null;
            }
        }
    }
}
