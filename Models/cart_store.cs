using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace S4Sales.Models
{
    /// <Note>
    // Cart CRUD -- the cart itself
    // CartItem CRUD - what goes into the cart
    ///</Note>
    
    public interface ICartStore<Cart>
    {
        void AddToCart(int hsmv_report_number);
        void ClearCart();
        string GetCartId();
        string MakeNewCart();
        bool IsACart();
        IEnumerable<CartItem> GetCartContent();
    }
    public class CartStore : ICartStore<Cart>
    {
        private readonly string _conn;
        private readonly SessionUtility _session;
        public CartStore(IConfiguration config, SessionUtility session)
        {
            _conn = config.GetConnectionString("tc_dev");
            _session = session;
        }

        public void AddToCart(int hsmv_report_number)
        {
            throw new NotImplementedException();
        }

        public string MakeNewCart()
        {
            if (!IsACart())
            {
                Cart new_cart = new Cart()
                {
                    cart_id = Guid.NewGuid().ToString(),
                    created_date = DateTime.Now
                };
                _session.SetSession("cart_id", new_cart.cart_id);
                return new_cart.cart_id;
            }
            return _session.GetSession("cart_id");
        }
        public string GetCartId()
        {
            return _session.GetSession("cart_id");
        }

        public void ClearCart()
        {
            _session.RemoveKey("cart_id");
        }

        public bool IsACart()
        {
            return _session.GetSession("cart_id") != null ? true : false;
        }

        public IEnumerable<CartItem> GetCartContent()
        {
            var cart = GetCartId();
            
            var queryText = "";  // TODO
            using(var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CartItem>(queryText);
            }
        }

    }
}