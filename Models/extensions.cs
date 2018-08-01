using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace S4Sales.Models
{
    /// <Note>
    // Utility class to interact with session state
    ///</Note> 


    public class SessionUtility
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public SessionUtility(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string key, string value)
        {
            HttpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            return HttpContextAccessor.HttpContext.Session.GetString(key);
        }
        public void RemoveKey(string key)
        {
            HttpContextAccessor.HttpContext.Session.SetString(key, null);
        }
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }

}