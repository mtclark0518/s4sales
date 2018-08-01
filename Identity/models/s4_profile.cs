using System;

namespace S4Sales.Identity
{
    public class S4Profile
    {
        public string s4_profile_id { get; internal set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool active { get; set; }
        // public bool approved { get; internal set; }
        // public DateTime approval_date {get; internal set;}
        // public bool verified { get; internal set; }
        // public DateTime verified_date {get; internal set;}
        public string identity { get; internal set;}
        public S4Profile(string s4id)
        {
            s4_profile_id = Guid.NewGuid().ToString();
            active = true;
            identity = s4id;
        }
    }
}