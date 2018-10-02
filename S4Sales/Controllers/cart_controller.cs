using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;
using S4Sales.Services;
using S4Sales.Log;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CartController: Controller
    {
        private readonly SessionUtility _session;
        private readonly CartStore _cart;
        private readonly Logg _log;        
        public CartController(CartStore c, SessionUtility s, Logg l)
        {
            _cart = c;
            _session = s;
            _log = l;
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
                    // record the addition to the cart
                    var details = new ActionLog()
                    {
                        action = Log.Action.Add,
                        target = item,
                        cart_id = req.cart_id
                    };
                    _log.Action(details);

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
                message = @"Your trying to access a cart that 
                isn't yours or the session isn't active bruh. 
                Or maybe the cart didn't add the item correctly."
            };
            return Task.FromResult(error);
        }


        ///<Note>
        // called when user navigates to checkout
        // returns duh
        ///</Note>
        [HttpGet("content")]
        public IEnumerable<CartItem> GetCartContent()
        {
            return _cart.GetContent(Request.Headers["cart_id"]);
        }


        ///<Note>
        // called by the cart component on init
        // double checks the session for an existing cart id
        // sends back existing / new cart
        ///</Note>
        [HttpGet("init")]
        public string InitializeCart()
        {
            // TODO
            // check session expiration
            // currently this just returns true
            // needs time check in session utility
            if(_session.IsValid())
            {
                
                if(!_cart.NeedACart()) // kind of a double negative but checks session for cart-id
                { 
                    return _session.GetSession("cart"); 
                } 
                
                if( _cart.MakeNewCart().Result == true) // hit when starting new session
                {
                    IPAddress ip = Request.HttpContext.Connection.LocalIpAddress;
                    var details = new SessionLog() // log the session information
                    {
                        cart_id = _session.GetSession("cart"),
                        session_id = _session.CurrentSession(),
                        client_ip = ip.ToString()
                    };
                    _log.Session(details);
                    return details.cart_id; // return with the cart id 
                }
                // TODO error creating the new cart
            }
            var error = "you messed up"; // error in session. figure out how to reset
            return error;
        }
    }
}