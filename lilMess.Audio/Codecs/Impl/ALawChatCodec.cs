namespace lilMess.Audio.Codecs.Impl
{
    using System;

    using NAudio.Codecs;
    using NAudio.Wave;

    public class ALawChatCodec
    {
        public string Name { get { return "G.711 a-law"; } }

        public int BitsPerSecond { get { return RecordFormat.SampleRate * 8; } }

        public WaveFormat RecordFormat { get { return new WaveFormat(8000, 16, 1); } }

        public bool IsAvailable { get { return true; } }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            byte[] encoded = new byte[length / 2];
            int outIndex = 0;
            for (int n = 0; n < length; n += 2)
            {
                encoded[outIndex++] = ALawEncoder.LinearToALawSample(BitConverter.ToInt16(data, offset + n));
            }
            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            byte[] decoded = new byte[length * 2];
            int outIndex = 0;
            for (int n = 0; n < length; n++)
            {
                short decodedSample = ALawDecoder.ALawToLinearSample(data[n + offset]);
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