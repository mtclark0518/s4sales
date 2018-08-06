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
        public DateTime created_date { get; set; }
        public string session_id { get; set; }
        public IEnumerable<CartItem> cart_content {get;set;}
        // constructor
        public Cart(){}
    }
    public class CartItem
    {
        public int hsmv_report_number {get;set;}
        public string cart {get;set;}
        public DateTime add_date {get;set;}
        // default constructor
        public CartItem() {}
        public CartItem(int hsmv)
        {
            hsmv_report_number = hsmv;
        }
    }
}