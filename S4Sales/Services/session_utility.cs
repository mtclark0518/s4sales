using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace S4Sales.Services
{
    /// <summary>
    // Utility class to interact with session state
    ///</summary> 


    public class SessionUtility
    {
        private readonly IHttpContextAccessor _httpCtx;

        public SessionUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpCtx = httpContextAccessor;
        }
        public bool IsValid()
        {
            return true;
        }
        public void SetSession(string key, string value)
        {
            _httpCtx.HttpContext.Session.SetString(key, value);
        }
        public string CurrentSession()
        {
            return _httpCtx.HttpContext.Session.Id;
        }
        public string GetSession(string key)
        {
            return _httpCtx.HttpContext.Session.GetString(key);
        }
        public void RemoveKey(string key)
        {
            _httpCtx.HttpContext.Session.SetString(key, null);
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