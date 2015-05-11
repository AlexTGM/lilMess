namespace lilMess.Misc
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class Serializer<T>
    {
        public static byte[] SerializeObject(object obj)
        {
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                var result = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(result, 0, (int)stream.Length);
                return result;
            }
        }

        public static T DeserializeObject(byte[] data)
        {
            if (data == null) { return default(T); }

            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream(data)) { return (T)formatter.Deserialize(stream); }
        }
    }
}