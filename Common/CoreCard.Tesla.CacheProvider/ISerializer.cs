using System.Text;
using Newtonsoft.Json;
using System;
namespace CoreCard.Telsa.Cache
{
    public interface ISerializer
    {
        byte[] Serialize(object item);
        T Deserialize<T>(byte[] serializedObject);
    }

    public class NewtonsoftSerializer : ISerializer
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private readonly JsonSerializerSettings settings;
        public T Deserialize<T>(byte[] serializedObject)
        {
            var jsonString = encoding.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }

        public byte[] Serialize(object item)
        {
            var type = item?.GetType();
            var jsonString = JsonConvert.SerializeObject(item, type, settings);
            return encoding.GetBytes(jsonString);
        }

        public NewtonsoftSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings ?? new JsonSerializerSettings();
        }
        public NewtonsoftSerializer() : this(null)
        {
        }
    }
}