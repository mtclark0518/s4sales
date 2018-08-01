using System;

namespace S4Sales.Identity
{    
    public class Organization
    {
        public string organization_id { get; set;}
        public string name {get;set;}
        public bool active { get; set; }
        public bool approved { get;set;}
        public bool pending { get;set;}
        public string approved_by { get; set;}
        public DateTime approved_date {get; set;}
        public Organization(string org_name)
        {
            organization_id = Guid.NewGuid().ToString();
            name = org_name;
            active = true;
            approved_by = string.Empty;
            approved_date = DateTime.Now;
        }
    }
    public class OrganizationMember
    {
        public string organization { get; internal set;}
        public string member { get; internal set; }
        public bool admin { get; internal set; }
    }
}