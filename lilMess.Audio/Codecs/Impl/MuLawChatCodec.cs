namespace lilMess.Audio.Codecs.Impl
{
    using System;

    using lilMess.Audio.Decoders.Impl;
    using lilMess.Audio.Encoders.Impl;

    using NAudio.Wave;

    internal class MuLawChatCodec : INetworkChatCodec
    {
        public string Name { get { return "G.711 mu-law"; } }

        public int BitsPerSecond { get { return RecordFormat.SampleRate * 8; } }

        public WaveFormat RecordFormat { get { return new WaveFormat(8000, 16, 1); } }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            var encoded = new byte[length / 2];
            var outIndex = 0;

            for (var n = 0; n < length; n += 2)
            {
                encoded[outIndex++] = MuLawEncoder.Encode(BitConverter.ToInt16(data, offset + n));
            }

            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            var decoded = new byte[length * 2];
            var outIndex = 0;

            for (var n = 0; n < length; n++)
            {
                var decodedSample = MuLawDecoder.Decode(data[n + offset]);
                decoded[outIndex++] = (byte)(decodedSample & 0xFF);
                decoded[outIndex++] = (byte)(decodedSample >> 8);
            }

            return decoded;
        }

        public void Dispose()
        {
        }
    }
}