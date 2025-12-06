using Microsoft.Data.SqlClient;
using RewearApi.BL;
using System;
using System.Collections.Generic;

namespace RewearApi.DAL
{
    public class OrderDAL : DBServices
    {
        private const string CON_STR_NAME = "RewearDB";

        private const string SP_GET_ALL = "GetAllOrders";
        private const string SP_GET_BY_ID = "GetOrderById";
        private const string SP_ADD = "AddOrder";
        private const string SP_UPDATE = "UpdateOrder";


        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    SqlCommand cmd = CreateCommand(SP_GET_ALL, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrder(reader));
                        }
                    }
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Order? GetOrderById(int orderId)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@OrderId", orderId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_ID, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapOrder(reader);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddOrder(Order order)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId",        order.ItemId },
                        { "@BuyerUserId",   order.BuyerUserId },
                        { "@SellerUserId",  order.SellerUserId },
                        { "@OrderDate",     order.OrderDate },
                        { "@TotalAmount",   order.TotalAmount },
                        { "@Status",        order.Status }
                    };

                    SqlCommand cmd = CreateCommand(SP_ADD, con, paramDic);

                    object result = cmd.ExecuteScalar();
                    int newId = Convert.ToInt32(result);
                    return newId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateOrder(Order order)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@OrderId",      order.OrderId },
                        { "@Status",       order.Status },
                        { "@TotalAmount",  order.TotalAmount },
                        { "@OrderDate",    order.OrderDate }
                    };

                    SqlCommand cmd = CreateCommand(SP_UPDATE, con, paramDic);

                    object result = cmd.ExecuteScalar();
                    int rows = Convert.ToInt32(result);
                    return rows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Order MapOrder(SqlDataReader reader)
        {
            Order o = new Order();

            o.OrderId = Convert.ToInt32(reader["OrderId"]);
            o.ItemId = Convert.ToInt32(reader["ItemId"]);
            o.BuyerUserId = Convert.ToInt32(reader["BuyerUserId"]);
            o.SellerUserId = Convert.ToInt32(reader["SellerUserId"]);

            o.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
            o.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
            o.Status = reader["Status"].ToString()!;

            try
            {
                int ordItemTitle = reader.GetOrdinal("ItemTitle");
                if (!reader.IsDBNull(ordItemTitle))
                    o.ItemTitle = reader.GetString(ordItemTitle);
            }
            catch (IndexOutOfRangeException) { }

            try
            {
                int ordBuyerFullName = reader.GetOrdinal("BuyerFullName");
                if (!reader.IsDBNull(ordBuyerFullName))
                    o.BuyerFullName = reader.GetString(ordBuyerFullName);
            }
            catch (IndexOutOfRangeException) { }

            try
            {
                int ordSellerFullName = reader.GetOrdinal("SellerFullName");
                if (!reader.IsDBNull(ordSellerFullName))
                    o.SellerFullName = reader.GetString(ordSellerFullName);
            }
            catch (IndexOutOfRangeException) { }

            return o;
        }
    }
}
