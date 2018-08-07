using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;
using S4Sales.Services;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CartController: Controller
    {
        private readonly SessionUtility _session;
        private readonly CartStore _cart;
        public CartController(CartStore cart, SessionUtility sess)
        {
            _cart = cart;
            _session = sess;
        }

        [HttpPost("add")]
        public Task<StandardResponse> AddToCart([FromBody]CartItemRequest req)
        {
            // irrelevant currently
            // verify the session we want to access
            var from = Request.HttpContext.Session.Id;
            // seems to be the place to make sure the item isn't already in the cart
            //make sure the request is for the correct cart
            if(req.cart_id == _cart.GetCart())
            {
                var item = int.Parse(req.hsmv);
                var canAdd = _cart.AddToCart(item);
                if(canAdd)
                {
                    var result = new StandardResponse()
                    {
                        code = StatusCode(200),
                        message = "success"
                    };
                    return Task.FromResult(result);
                }
            }
            
            var error = new StandardResponse()
            {
                code = StatusCode(500),
                message = @"Your trying to access a cart that 
                isn't yours or isn't active bruh. Or maybe it didn't add the item correctly."
            };
            return Task.FromResult(error);
        }

        // retrieves items for a given cart
        [HttpGet("content")]
        public IEnumerable<CartItem> GetCartContent()
        {
            var cart = Request.Headers["cart_id"];
            var content = _cart.GetContent(cart);
            return content;
        }

        [HttpGet("init")]
        public string InitializeCart()
        {
            // check session expiration
            // currently this just returns true
            // needs time check in session utility
            if(_session.IsValid())
            {
                // if we've searched already grab our cart
                if(!_cart.NeedACart()) 
                { 
                    var result = _session.GetSession("cart"); 
                    return result;
                } 
                // add a new cart to session and db log
                if( _cart.MakeNewCart().Result == true)
                {
                    var result = _session.GetSession("cart"); 
                    return result;
                }
                // error creating the new cart
            }
            // error in session. figure out how to reset
            var error = "you messed up";
            return error;
        }
    }
}