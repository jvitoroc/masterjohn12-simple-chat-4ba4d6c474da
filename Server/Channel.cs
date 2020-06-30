using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class Channel
    {
        private List<Client> clients = new List<Client>();
        private string name;
        private string id;

        public string Name { get => name; set => name = value; }
        public string Id { get => id; }
        public int ClientCount { get { return clients.Count; } }

        public Channel(string name = "")
        {
            this.name = name;
            this.id = Guid.NewGuid().ToString();
        }

        public void AddClient(Client client)
        {
            if (!HasClient(client))
            {
                this.clients.Add(client);
                client.JoinChannel(this);
                UpdateChannelUsers();
                NotifyUserJoined(client);
            }
        }

        public string GetUsernamesAsCSV()
        {
            string value = "";
            for (int i = 0; i < clients.Count; i++)
            {
                value += clients[i].Username + ",";
            }

            if (value.Length > 0)
                value = value.Substring(0, value.Length - 1);

            return value;
        }

        public void DisconnectClient(Client client)
        {
            if (HasClient(client))
            {
                this.clients.Remove(client);
                client.LeaveChannel();
                UpdateChannelUsers();
            }
        }

        public bool HasClient(Client client)
        {
            return this.clients.Contains(client);
        }

        public void UpdateChannelUsers()
        {
            Message m = new Message();
            m.Request = "update_channel_users";
            m.SetHeaderValue("usernames", GetUsernamesAsCSV());

            BroadcastNotification(m);
        }

        private void NotifyUserJoined(Client client)
        {
            Message m = new Message();
            m.Request = "user_joined";
            m.SetHeaderValue("username", client.Username);
            m.message = "O usuário " + client.Username + " entrou no canal.";

            BroadcastNotification(m);
        }

        public void BroadcastAudioData(byte[] data)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].SendAudioData(data);
            }
        }

        public void BroadcastNotification(Message message)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Notify(message);
            }
        }

        public Task BroadcastNotificationAsync(Message message)
        {
            return Task.Run(() =>
            {
                BroadcastNotification(message);
            });
        }
    }
}
