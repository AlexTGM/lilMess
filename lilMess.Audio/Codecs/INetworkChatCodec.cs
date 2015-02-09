namespace lilMess.Audio.Codecs
{
    using System;

    using NAudio.Wave;

    public interface INetworkChatCodec : IDisposable
    {
        string Name { get; }

        int BitsPerSecond { get; }

        WaveFormat RecordFormat { get; }

        byte[] Encode(byte[] data, int offset, int length);

        byte[] Decode(byte[] data, int offset, int length);
    }
}