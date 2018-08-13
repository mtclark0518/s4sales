using System;

namespace S4Sales.Models
{
    public class AccountProfile
    {
        public string s4_profile_id { get; internal set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool active { get; set; }
        public string identity { get; internal set;}
        public AccountProfile(string s4id)
        {
            s4_profile_id = Guid.NewGuid().ToString();
            active = true;
            identity = s4id;
        }
    }
}