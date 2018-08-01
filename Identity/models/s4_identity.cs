using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Identity
{
    public class S4Identity : S4IdentityBase
    {
        public  S4Identity() : base() { }
        public S4Identity(string name) : base(name) { }
    }
    public abstract class S4IdentityBase
    {
        private readonly IList<S4IdentityRole> _roles;
        public S4IdentityBase()
        {
            _roles = new List<S4IdentityRole>();
            created_on = new DateTime();
        }
        public S4IdentityBase(string name) : this()
        {
            s4_id = Guid.NewGuid().ToString();
            user_name = name;
            password_salt = S4PasswordHasher<S4IdentityBase>.GenerateSalt();
            active = true;
            created_on = DateTime.Now;
            s4roles = _roles;
        }

        [Key]
        public string s4_id { get; set; }
        public string user_name { get; set; }
        public string normalized_user_name { get; internal set; }
        public string email { get; set; }
        public string normalized_email { get; internal set; }
        public string password_salt { get; private set; }
        public string password_hash { get; internal set; }
        public bool active {get; internal set;}
        public DateTime created_on { get; internal set; }
        public IList<S4IdentityRole> s4roles 
        {
            get
            {  
                return _roles.ToList().AsReadOnly();
            }

            internal set
            { 
                _roles.Clear(); 

                if (value != null) 
                {
                    _roles.ToList().AddRange(value);
                }
            }
        }
        internal void AddRole(S4IdentityRole role)
        {
            if (role == null) { throw new ArgumentNullException(nameof(role)); }
            var has_role = _roles.Any(r => r.Equals(role));

            if(!has_role)
            {
                _roles.Add(role);
            }
        }
        internal void RemoveRole(S4IdentityRole role)
        {
            if(role == null){throw new ArgumentNullException(nameof(role));}
            s4roles = _roles.Where(r => !r.Equals(role)).ToList();
        }
    }
}

