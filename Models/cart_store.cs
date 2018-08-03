using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace S4Sales.Models
{
    /// <Note>
    // Cart CRUD -- the cart itself
    // CartItem CRUD - what goes into the cart
    ///</Note>

    public class CartStore
    {
        private readonly string _conn;
        private readonly SessionUtility _session;
        public CartStore(IConfiguration config, SessionUtility session)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _session = session;
        }

        public bool AddToCart(int hsmv)
        {
            var cart_ref = GetCart();
            CartItem item = new CartItem(hsmv)
            {
                cart = cart_ref,
                add_date = DateTime.Now
            };

            var _query = $@"INSERT into cart_item VALUES(@hsmv, @cart, @date)";
            var _params = new 
            {
                hsmv = item.hsmv_report_number,
                cart = item.cart,
                date = item.add_date
            };

            using (var conn = new NpgsqlConnection(_conn))
            {
                var added = conn.Execute(_query, _params);
                return added == 1;
            }
        }
        
        public string GetCart()
        {
            return _session.GetSession("cart");
        }

        public IEnumerable<CartItem> GetCartContent()
        {
            var cart_id = GetCart();
            
            var _query = "SELECT * FROM cart_item WHERE cart = @cart";
            var _params = new {cart = cart_id};
            using(var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CartItem>(_query, _params);
            }
        }

        public bool MakeNewCart()
        {
            if (!IsACart())
            {
                Cart new_cart = new Cart()
                {
                    cart_id = Guid.NewGuid().ToString(),
                    created_date = DateTime.Now,
                    session_id = _session.CurrentSession()
                };
                var _query = $@"INSERT INTO cart VALUES (@cart, @session, @date)";
                var _params = new 
                {
                    cart = new_cart.cart_id,
                    session = new_cart.session_id,
                    date = new_cart.created_date
                };
                using (var conn = new NpgsqlConnection(_conn))
                {
                    var cart = conn.Execute(_query, _params);
                    if(cart == 1)
                    {
                        _session.SetSession("cart", new_cart.cart_id);
                        return true;
                    }
                }
            }
            // should add some feedback, but basically unless everything works we fails yo
            return false;
        }

        public void RemoveCart()
        {
            _session.RemoveKey("cart");
        }

        public bool IsACart()
        {
            return _session.GetSession("cart") != null ? true : false;
        }
    }
}