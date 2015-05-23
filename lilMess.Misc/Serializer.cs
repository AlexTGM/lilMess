namespace lilMess.Misc
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using Newtonsoft.Json;

    public static class Serializer<T>
    {
        public static byte[] SerializeObject(object obj)
        {
            //return GetBytes(JsonConvert.SerializeObject(obj));

            var serializeObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
                                                                                               {
                                                                                                   TypeNameHandling = TypeNameHandling.Objects,
                                                                                                   TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                                                                                               });
            return GetBytes(serializeObject);



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
            // return JsonConvert.DeserializeObject<T>(GetString(data));

            return JsonConvert.DeserializeObject<T>(GetString(data), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            //if (data == null) { return default(T); }

            //var formatter = new BinaryFormatter();

            //using (var stream = new MemoryStream(data)) { return (T)formatter.Deserialize(stream); }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}