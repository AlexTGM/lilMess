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
            this.codec = new UncompressedPcmChatCodec();

            this.waveIn = new WaveIn { BufferMilliseconds = 500, DeviceNumber = 0, WaveFormat = this.codec.RecordFormat, };

            this.waveOut = new WaveOut();
            this.waveProvider = new BufferedWaveProvider(this.codec.RecordFormat);
            this.waveOut.Init(this.waveProvider);

            this.waveOut.Play();
        }

        public void StartRecording(SendVoiceMessage sendVoiceMessageDelegate)
        {
            if (this.recording) return;

            this.sendVoiceMessage = sendVoiceMessageDelegate;

            this.waveIn.StartRecording();
            this.waveIn.DataAvailable += this.WaveInDataAvailable;
            this.recording = true;
        }

        public void StopRecording()
        {
            if (!this.recording) return;

            this.waveIn.StopRecording();
            this.waveIn.DataAvailable -= this.WaveInDataAvailable;
            this.recording = false;
        }

        public void Translate(byte[] message)
        {
            var decoded = this.codec.Decode(message, 0, message.Length);
            this.waveProvider.AddSamples(decoded, 0, decoded.Length);
        }

        public void Shutdown()
        {
            this.waveOut.Stop();
            this.waveIn.Dispose();
            this.waveOut.Dispose();
            this.codec.Dispose();
        }

        public void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded == 0) return;
            var encoded = this.codec.Encode(e.Buffer, 0, e.BytesRecorded);
            this.sendVoiceMessage(encoded);
        }
    }
}