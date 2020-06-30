using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using NAudio.Wave;

namespace Client
{ 
    class ServerCommunication
    {
        private AudioDriver audioDriver;
        private UDPDuplexCommunication audio = null;
        private TCPDuplexCommunication text;

        private Guid id;

        public ServerCommunication(TcpClient client) {
            id = Guid.Empty;
            text = new TCPDuplexCommunication(client);
            text.MessageReceived += OnTextMessageReceived;
            text.StartListening();

            UdpClient udpClient = new UdpClient();
            udpClient.Connect("localhost", 13001);
            audio = new UDPDuplexCommunication(udpClient, (IPEndPoint)client.Client.RemoteEndPoint);

            audioDriver = new AudioDriver();
            audioDriver.StartRecording();
            audioDriver.DataAvailable += OnAudioDataAvailable;
        }

        private void OnAudioDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            audio.WriteMessage(waveInEventArgs.Buffer);
        }

        private void OnTextMessageReceived(byte[] message)
        {
            Message m = Message.Convert(message);
            Console.WriteLine(m.message);
            //DefineID(m);
        }

        private void OnAudioDataReceived(byte[] message)
        {

        }

        public void SendTextMessage(string message)
        {
            Message m = new Message();
            m.message = message;
            m.headers.Add("type", "message");
            SendTextMessage(m);
        } 

        public void SendTextMessage(Message message)
        {
            SendMessage(Message.Convert(message));
        }

        private void SendMessage(byte[] message)
        {
            text?.WriteMessage(message);
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
                
                //
                audio.StartListening();
                audio.WriteMessage(Encoding.ASCII.GetBytes(id.ToString()));
            }
        }
    }
}
