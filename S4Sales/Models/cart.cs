using System;
using System.Collections.Generic;

namespace S4Sales.Models
{
    /// <Note>
    // model definitions for shopping cart and shopping cart item
    ///</Note>
    
    public class Cart
    {
        public string cart_id { get; set; }
        public string session_id { get; set; }
        public DateTime created_date { get; set; }
        // constructor
        public Cart(){}
    }
    public class CartItem
    {
        public DateTime add_date {get;set;}
        public string cart {get;set;}
        public int cost = 1025; // cost in us cents
        public int hsmv_report_number {get;set;}
        // default constructor
        public CartItem() {}
        public CartItem(int hsmv)
        {
            hsmv_report_number = hsmv;
        }
    }
}