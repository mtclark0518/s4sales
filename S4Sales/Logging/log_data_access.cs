using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace S4Sales.Log
{
    public class Logg
    {
        private readonly string _conn;
        public Logg(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
        // possibly quicker way to get connections
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_conn);
        }

        // after use function
        private void Dispose(NpgsqlConnection conn)
        {
            conn.Dispose();
        }

        public void Action(ActionLog log)
        {
            var _query = $@"
                INSERT INTO log_action 
                (cart_id, action, target, timestamp)
                VALUES(@cart, @action, @target, @date)";
            var _params = new 
            {
                cart = log.cart_id,
                action = log.action,
                target = log.target,
                date = DateTime.Now
            };
            var conn = GetConnection();
            conn.Execute(_query, _params);
            Dispose(conn);
        }
        public void Query(QueryLog log)
        {

            var _query = $@"
                INSERT INTO log_query 
                (cart_id, method, parameters, succeeded, return_count, timestamp)
                VALUES(@cart, @method, @params, @suc, @count, @date)";
            var _params = new 
            {
                cart = log.cart_id, 
                method = log.method,
                parameters = log.parameters, 
                suc = log.succeeded, 
                count = log.return_count, 
                date = DateTime.Now
            };
            
            var conn = GetConnection();
            conn.Execute(_query, _params);
            Dispose(conn);
        }
        public void Session(SessionLog log)
        {
            var _query = $@"
                INSERT INTO log_session 
                (session_id, client_ip, cart_id, timestamp)
                VALUES(@s, @ip, @c, @d)";
            var _params = new 
            {
                s = log.session_id,
                ip = log.client_ip,
                c = log.cart_id,
                d = DateTime.Now
            };
            var conn = GetConnection();
            conn.Execute(_query, _params);
            Dispose(conn);
        }


    }
}