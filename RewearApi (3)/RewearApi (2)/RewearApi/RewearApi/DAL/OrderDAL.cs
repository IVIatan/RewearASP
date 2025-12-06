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
        private const string SP_GET_BY_BUYER = "GetOrdersByBuyerUserId";
        private const string SP_GET_BY_SELLER = "GetOrdersBySellerUserId";
        private const string SP_ADD = "AddOrder";
        private const string SP_UPDATE = "UpdateOrder";

        public List<Order> GetAll()
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

        public Order? GetById(int orderId)
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

        public List<Order> GetByBuyer(int buyerUserId)
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@BuyerUserId", buyerUserId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_BUYER, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapSimpleOrder(reader));
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

        public List<Order> GetBySeller(int sellerUserId)
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@SellerUserId", sellerUserId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_SELLER, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapSimpleOrder(reader));
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

        public int Add(Order order)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId",       order.ItemId },
                        { "@BuyerUserId",  order.BuyerUserId },
                        { "@SellerUserId", order.SellerUserId },
                        { "@TotalAmount",  order.TotalAmount },
                        { "@Status",       order.Status }
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

        public int Update(Order order)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@OrderId",      order.OrderId },
                        { "@ItemId",       order.ItemId },
                        { "@BuyerUserId",  order.BuyerUserId },
                        { "@SellerUserId", order.SellerUserId },
                        { "@TotalAmount",  order.TotalAmount },
                        { "@Status",       order.Status }
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

            if (ColumnExists(reader, "ItemTitle") && reader["ItemTitle"] != DBNull.Value)
                o.ItemTitle = reader["ItemTitle"].ToString();

            if (ColumnExists(reader, "BuyerFullName") && reader["BuyerFullName"] != DBNull.Value)
                o.BuyerFullName = reader["BuyerFullName"].ToString();

            if (ColumnExists(reader, "SellerFullName") && reader["SellerFullName"] != DBNull.Value)
                o.SellerFullName = reader["SellerFullName"].ToString();

            return o;
        }

        private Order MapSimpleOrder(SqlDataReader reader)
        {
            Order o = new Order();

            o.OrderId = Convert.ToInt32(reader["OrderId"]);
            o.ItemId = Convert.ToInt32(reader["ItemId"]);
            o.BuyerUserId = Convert.ToInt32(reader["BuyerUserId"]);
            o.SellerUserId = Convert.ToInt32(reader["SellerUserId"]);
            o.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
            o.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
            o.Status = reader["Status"].ToString()!;

            return o;
        }

        private bool ColumnExists(SqlDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}
