using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Common;
using NAudio.Wave;
using UDP;

namespace ClientConsole
{ 
    public class Server : BaseCommunication
    {
        private string username = "";
        private AudioManager audioDriver;

        public string Username { get => username; }
        public Action ChannelChanged;
        public Action<string[]> ChannelUsersUpdated;
        public Action<string, string> TextMessageReceived;

        public Server(TcpClient tcpClient) : base(tcpClient)
        {
            RegisterMessageHandler(ReceiveTextMessage);
            RegisterMessageHandler(ChannelChange);
            RegisterMessageHandler(UpdateChannelUsers);

            IPEndPoint audioIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13001);

            audioDriver = new AudioManager();
            audioDriver.StartRecording();
            audioDriver.StartListening();
            audioDriver.DataAvailable += OnAudioDataAvailable;

            UDPClient udpClient = new UDPClient();
            udpClient.Connect(audioIpEndPoint);
            audioSocket = new UDPDuplexCommunication(udpClient);
            audioSocket.MessageReceived += OnAudioDataReceived;
            audioSocket.StartListening();
        }

        private void OnAudioDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            audioSocket.WriteMessage(waveInEventArgs.Buffer);
        }

        [Request("channel_change")]
        private void ChannelChange(Message message, RespondMessage respondMessage)
        {
            Message m;
            if (ChannelChanged != null)
            {
                ChannelChanged();
                m = Message.CreateSucessResponse();  
            }
            else
            {
                m = Message.CreateUnfulfilledResponse();
            }
            respondMessage(m);
        }

        [Request("notification")]
        private void Notification(Message message, RespondMessage respondMessage)
        {
            Message m = Message.CreateSucessResponse();
            respondMessage(m);
        }

        [Request("channel_leave")]
        private void ChannelLeave(Message message, RespondMessage respondMessage)
        {
            Message m = Message.CreateSucessResponse();
            respondMessage(m);
        }

        [Request("update_channel_users")]
        private void UpdateChannelUsers(Message message, RespondMessage respondMessage)
        {
            if (ChannelUsersUpdated != null)
                ChannelUsersUpdated(message.message.Split(','));

            Message m = Message.CreateSucessResponse();
            m.message = "Channel's users updated sucessfully";
            respondMessage(m);
        }

        [Request("text_message")]
        private void ReceiveTextMessage(Message message, RespondMessage respondMessage)
        {
            if (TextMessageReceived != null)
                TextMessageReceived(message.message, message.GetHeaderValue("from"));
        }

        protected override void OnAudioDataReceived(byte[] message)
        {
            audioDriver.Feed(message, message.Length);
        }

        public void RegisterMyself(string username)
        {
            Message m = new Message();
            m.headers.Add("request", "register_myself");
            m.headers.Add("username", username);
            this.username = username;
            Request(m);
        }

        public void CreateChannel()
        {
            Message m = new Message();
            m.headers.Add("request", "create_channel");
            Request(m);
        }

        public void JoinChannel(string channelId)
        {
            Message m = new Message();
            m.headers.Add("request", "join_channel");
            m.headers.Add("channel_id", channelId);
            Request(m);
        }

        public void SendTextMessage(string message)
        {
            Message m = new Message();
            m.message = message;
            m.headers.Add("request", "text_message");
            m.headers.Add("from", username);
            Request(m);
        }
    }
}
