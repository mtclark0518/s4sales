using System;

namespace S4Sales.Identity
{    
    public class S4IdentityRole
    {
        public string s4_role_name { get; internal set; }
        public string normalized_role_name { get; internal set; }

        public bool Equals(S4IdentityRole other) 
        { 
            return s4_role_name == other.s4_role_name; 
        }
        public bool Equals(string other_role_name) 
        { 
            return s4_role_name == other_role_name; 
        }
        public S4IdentityRole(string name)
        {
            s4_role_name = name;
            normalized_role_name = name.ToLower();
        }
        public S4IdentityRole(){}
    }


}