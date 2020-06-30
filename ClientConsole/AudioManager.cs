using Common;
using NAudio.Wave;

namespace ClientConsole
{
    public class AudioManager
    {
        private bool listening = false;
        private bool recording = false;

        private WaveInEvent recorder;

        private BufferedWaveProvider playerBufferedWaveProvider;
        private WaveOutEvent player;

        public delegate void DataAvailableEventHandler(object sender, WaveInEventArgs waveInEventArgs);
        public event DataAvailableEventHandler DataAvailable;

        public AudioManager()
        {

        }

        public void StartRecording()
        {
            if (!recording)
            {
                recorder = new WaveInEvent();
                //recorder.DeviceNumber = 0;
                recorder.BufferMilliseconds = 50;
                recorder.DataAvailable += OnDataAvailable;
                recorder.StartRecording();
                recording = true;
            }
        }

        public void StartListening()
        {
            if (!listening)
            {
                player = new WaveOutEvent();
                player.Volume = 1;
                playerBufferedWaveProvider = new BufferedWaveProvider((new WaveInEvent()).WaveFormat);
                player.Init(playerBufferedWaveProvider);
                player.Play();
                listening = true;
            }
        }

        public void StopRecording()
        {
            if (recording)
            {
                recorder.Dispose();
                recording = false;
            }
        }

        public void StopListening()
        {
            if (listening)
            {
                player.Dispose();
                playerBufferedWaveProvider = null;
                listening = false;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            DataAvailable?.Invoke(sender, waveInEventArgs);
        }

        public void Feed(byte[] audioBuffer, int audioLength)
        {
            if (listening)
                playerBufferedWaveProvider.AddSamples(audioBuffer, 0, audioLength);
        }
    }
}
