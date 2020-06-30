using NAudio.CoreAudioApi.Interfaces;

namespace Common
{
    public abstract class BaseDuplexCommunication
    {
        public delegate void MessageReceivedEventHandler(byte[] message);
        public event MessageReceivedEventHandler MessageReceived;

        public virtual bool Connected { get; }

        public BaseDuplexCommunication()
        { }

        public abstract void Disconnect();

        protected virtual void OnMessageReceived(byte[] message) {
            MessageReceived?.Invoke(message);
        }
        public abstract void WriteMessage(byte[] message);
        public abstract void StartListening();
    }
}
