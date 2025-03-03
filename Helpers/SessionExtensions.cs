using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Convenience.org.Helpers
{
    public static class SessionExtensions
    {
        // Serialize an object to session
        public static void SetObject(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        // Deserialize an object from session
        public static T GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }

}
