using Common;
using System;
using System.Threading.Tasks;

namespace Server
{
    public class Client : BaseCommunication {
  
        private Channel channel;
        private string username;

        public Channel Channel { get => channel; }
        public string Username { get => username; }

        public Client(TCPDuplexCommunication tcpClient) : base(tcpClient)
        {
            RegisterMessageHandler(TextMessageReceived);
            RegisterMessageHandler(RegisterClient);
            RegisterMessageHandler(CreateChannel);
            RegisterMessageHandler(OnJoinChannel);
        }

        [Request("text_message")]
        private void TextMessageReceived(Message message, RespondMessage respondMessage)
        {
            channel?.BroadcastNotification(message);
            Message m = Message.CreateSucessResponse();
            m.message = "Text message sent sucessfully";
            respondMessage(m);
        }

        [Request("register_myself")]
        private void RegisterClient(Message message, RespondMessage respondMessage)
        {
            username = message.GetHeaderValue("username");
            Message m = Message.CreateSucessResponse();
            m.message = "Cliente registrado com sucesso";
            respondMessage(m);
        }

        [Request("create_channel")]
        private void CreateChannel(Message message, RespondMessage respondMessage)
        {
            Message m;
            try
            {
                Channel c = Program.CreateChannel();
                m = Message.CreateSucessResponse();
                m.message = "Canal criado com sucesso";
                m.SetHeaderValue("channel_id", c.Id);
            }
            catch(Exception e)
            {
                m = Message.CreateUnfulfilledResponse();
                m.message = "Um erro ocorreu: " + e.Message;
            }   
            respondMessage(m);
        }

        [Request("join_channel")]
        private void OnJoinChannel(Message message, RespondMessage respondMessage)
        {
            Message m;
            try
            {
                Program.JoinChannel(this, message.GetHeaderValue("channel_id"));
                m = Message.CreateSucessResponse();
                m.message = "Cliente entrou no canal com sucesso";
                m.SetHeaderValue("usernames", channel.GetUsernamesAsCSV());
                m.SetHeaderValue("channel_id", channel.Id);
            }
            catch (Exception e)
            {
                m = Message.CreateUnfulfilledResponse();
                m.message = "Um erro ocorreu: " + e.Message;
            }
            respondMessage(m);
        }

        protected override void OnAudioDataReceived(byte[] message)
        {
            channel?.BroadcastAudioData(message);
        }

        public void SendTextMessage(string message)
        {
            Message m = new Message();
            m.message = message;
            m.SetHeaderValue("request", "text_message");
            Request(m);
        }

        public void UpdateChannel()
        {
            Message m = new Message();
            if (channel == null)
            {
                m.SetHeaderValue("request", "channel_leave");
            }
            else
            {
                m.SetHeaderValue("request", "channel_change");
                m.SetHeaderValue("channel_id", channel.Id);
            }
            Request(m);
        }

        public void JoinChannel(Channel channel)
        {
            this.channel = channel;
            UpdateChannel();
        }

        public void LeaveChannel()
        {
            this.channel = null;
            UpdateChannel();
        }
    }
}
