using NAudio.Wave;

namespace Common
{
    public class AudioDriver
    {
        private bool listening = false;
        private bool recording = false;

        private WaveInEvent recorder;

        private BufferedWaveProvider playerBufferedWaveProvider;
        private WaveOut player;

        public delegate void DataAvailableEventHandler(object sender, WaveInEventArgs waveInEventArgs);
        public event DataAvailableEventHandler DataAvailable;

        public AudioDriver()
        {

        }

        public void StartRecording()
        {
            recorder = new WaveInEvent();
            recorder.DataAvailable += OnDataAvailable;
            recorder.StartRecording();
            recording = true;
        }

        public void StartListening()
        {
            player = new WaveOut();
            playerBufferedWaveProvider = new BufferedWaveProvider((new WaveInEvent()).WaveFormat);
            player.Init(playerBufferedWaveProvider);
            player.Play();
            listening = true;
        }

        public void StopRecording()
        {
            recorder.Dispose();
            recording = false;
        }

        public void StopListening()
        {
            player.Dispose();
            playerBufferedWaveProvider = null;
            listening = false;
        }

        private void OnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            DataAvailable?.Invoke(sender, waveInEventArgs);
        }

        public void Feed(Message message)
        {
            if (!listening)
                return;
            //if (message.type != PacketType.Audio)
            //    return;
            //playerBufferedWaveProvider.AddSamples(message.audioBuffer, 0, message.audioLength);
        }

        public void Feed(byte[] audioBuffer, int audioLength)
        {
            if (!listening)
                return;
            playerBufferedWaveProvider.AddSamples(audioBuffer, 0, audioLength);
        }
    }
}
