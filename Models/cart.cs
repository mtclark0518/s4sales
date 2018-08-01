using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S4Sales.Models
{
    /// <Note>
    // model definitions for shopping cart and shopping cart item
    ///</Note>
    
    public class Cart
    {
        [Key]
        public string cart_id { get; set; }
        public DateTime created_date { get; set; }
        public IEnumerable<CartItem> cart_content {get;set;}
    }
    public class CartItem
    {
        [Key]
        public string cart_item_key {get;set;}
        public int hsmv_report_number {get;set;}
        public string cart_id {get;set;}
        public DateTime created_date {get;set;}

    }
}