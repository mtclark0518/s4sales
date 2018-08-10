using System;
using System.Collections.Generic;

namespace S4Sales.Log
{
    public enum Action {
        Add,
        Remove,
        Clear,
        Checkout
    }

    public class ActionLog 
    {
        public string cart_id {get;set;}
        public Action action {get;set;}
        public int target {get;set;}
    }
    public class SessionLog 
    {
        public string session_id {get;set;}
        public string client_ip {get;set;}
        public string cart_id {get;set;}
    }

    public class QueryLog 
    {
        public string cart_id {get;set;}
        public string method {get;set;}
        public List<string> parameters {get;set;}
        public bool succeeded {get;set;}
        public int return_count {get;set;}
        public DateTime executed {get;set;}
    }
}