using System;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Client
{

    class ServerCommunication
    {
        private UDPDuplexCommunication audio;
        private TCPDuplexCommunication text;

        private Guid id;

        public ServerCommunication(TcpClient client) {
            id = Guid.Empty;
            text = new TCPDuplexCommunication(client);
            text.MessageReceived += OnTextMessageReceived;
            text.StartListening();
        }

        private void OnTextMessageReceived(byte[] message)
        {
            Message m = Message.Convert(message);

            DefineID(m);
        }

        private void OnAudioDataReceived(byte[] message)
        {

        }

        public void SendTextMessage(string message)
        {
            text?.WriteMessage(Encoding.UTF8.GetBytes(message));
        }

        public void SendAudioData(byte[] data)
        {
            audio?.WriteMessage(data);
        }

        private void DefineID(Message message)
        {
            string value = message.GetHeaderValue("define_id");
            if (value != "")
            {
                id = Guid.Parse(value);
                TryConnectAudio();
            }
        }

        private void TryConnectAudio()
        {
            if(id != Guid.Empty && audio == null)
            {
                UdpClient client = new UdpClient();
                client.Connect("localhost", 3001);
                audio = new UDPDuplexCommunication(client);
                audio.StartListening();
                audio.WriteMessage(Encoding.ASCII.GetBytes(id.ToString()));
            }
        }
    }
}
