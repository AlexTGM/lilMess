namespace lilMess.Audio
{
    using NAudio.Wave;

    public delegate void SendVoiceMessage(byte[] message);

    public interface IAudioProcessor
    {
        void StartRecording(SendVoiceMessage sendVoiceMessageDelegate);

        void StopRecording();

        void Translate(byte[] message);

        void Shutdown();

        void WaveInDataAvailable(object sender, WaveInEventArgs e);
    }
}