using Common;
using NAudio.Wave;
using System;

namespace ClientForm
{
    public class Server : BaseCommunication
    {
        private string username = "";
        private AudioManager audioDriver;

        public string Username { get => username; }
        public AudioManager AudioManager { get => audioDriver; }

        public Action ChannelChanged;
        public Action<string[]> ChannelUsersUpdated;
        public Action<string, string> TextMessageReceived;

        public Server()
        {
            Initialize();
        }

        public void Initialize()
        {
            RegisterHandlers();
            audioDriver = new AudioManager();
            audioDriver.DataAvailable += OnAudioDataAvailable;
        }

        public void RegisterHandlers()
        {
            RegisterNotificationHandler(ReceiveTextMessage);
            RegisterNotificationHandler(UpdateChannelUsers);
            RegisterMessageHandler(ChannelChange);
        }

        private void OnAudioDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            if (AudioConnected)
                audioSocket.WriteMessage(waveInEventArgs.Buffer);
        }

        protected override void OnAudioDataReceived(byte[] message)
        {
            if (AudioConnected)
                audioDriver.Feed(message, message.Length);
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

        [Request("channel_leave")]
        private void ChannelLeave(Message message, RespondMessage respondMessage)
        {
            Message m = Message.CreateSucessResponse();
            respondMessage(m);
        }

        [Request("update_channel_users")]
        private void UpdateChannelUsers(Message message)
        {
            if (ChannelUsersUpdated != null)
                ChannelUsersUpdated(message.GetHeaderValue("usernames").Split(','));
        }

        [Request("text_message")]
        private void ReceiveTextMessage(Message message)
        {
            if (TextMessageReceived != null)
                TextMessageReceived(message.message, message.GetHeaderValue("from"));
        }

        public Message RegisterMyself(string username)
        {
            Message m = new Message();
            m.SetHeaderValue("request", "register_myself");
            m.SetHeaderValue("username", username);
            this.username = username;
            return Request(m);
        }

        public Message CreateChannel()
        {
            Message m = new Message();
            m.SetHeaderValue("request", "create_channel");
            return Request(m);
        }

        public Message JoinChannel(string channelId)
        {
            Message m = new Message();
            m.SetHeaderValue("request", "join_channel");
            m.SetHeaderValue("channel_id", channelId);
            return Request(m);
        }

        public void SendTextMessage(string message)
        {
            Message m = new Message();
            m.message = message;
            m.SetHeaderValue("request", "text_message");
            m.SetHeaderValue("from", username);
            Request(m);
        }
    }
}
