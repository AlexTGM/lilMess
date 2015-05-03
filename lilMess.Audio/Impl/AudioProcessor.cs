namespace lilMess.Audio.Impl
{
    using lilMess.Audio.Codecs;
    using lilMess.Audio.Codecs.Impl;

    using NAudio.Wave;

    public class AudioProcessor : IAudioProcessor
    {
        private readonly INetworkChatCodec codec;

        private readonly WaveIn waveIn;

        private readonly WaveOut waveOut;

        private readonly BufferedWaveProvider waveProvider;

        private SendVoiceMessage sendVoiceMessage;

        private bool recording;

        public AudioProcessor()
        {
            codec = new UncompressedPcmChatCodec();

            waveIn = new WaveIn { BufferMilliseconds = 500, DeviceNumber = 0, WaveFormat = codec.RecordFormat };

            waveOut = new WaveOut();
            waveProvider = new BufferedWaveProvider(codec.RecordFormat);
            waveOut.Init(waveProvider);

            waveOut.Play();
        }

        public void StartRecording(SendVoiceMessage sendVoiceMessageDelegate)
        {
            if (recording) return;

            sendVoiceMessage = sendVoiceMessageDelegate;

            waveIn.StartRecording();
            waveIn.DataAvailable += WaveInDataAvailable;
            recording = true;
        }

        public void StopRecording()
        {
            if (!recording) return;

            waveIn.StopRecording();
            waveIn.DataAvailable -= WaveInDataAvailable;
            recording = false;
        }

        public void Translate(byte[] message)
        {
            var decoded = codec.Decode(message, 0, message.Length);
            waveProvider.AddSamples(decoded, 0, decoded.Length);
        }

        public void Shutdown()
        {
            waveOut.Stop();
            waveIn.Dispose();
            waveOut.Dispose();
            codec.Dispose();
        }

        public void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded == 0) return;
            var encoded = codec.Encode(e.Buffer, 0, e.BytesRecorded);
            sendVoiceMessage(encoded);
        }
    }
}