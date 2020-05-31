using System.Collections.Generic;
using Common;

namespace Server
{
    public class Channel
    {
        private List<ClientCommunication> clients = new List<ClientCommunication>();
        private string name;

        public Channel(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }
        
        public void AddClient(ClientCommunication client)
        {
            clients.Add(client);
        }

        public bool DisconnectClient(ClientCommunication client)
        {
            return clients.Remove(client);
        }

        public bool HasClient(ClientCommunication client)
        {
            return this.clients.Contains(client);
        }

        public void BroadcastAudioData(byte[] data)
        {
            foreach(ClientCommunication cm in clients)
            {
                cm.SendAudioData(data);
            }
        }

        public void BroadcastTextMessage(byte[] message)
        {
            foreach (ClientCommunication cm in clients)
            {
                cm.SendTextMessage(message);
            }
        }

        public void BroadcastTextMessage(string message)
        {
            foreach (ClientCommunication cm in clients)
            {
                cm.SendTextMessage(message);
            }
        }

    }
}
