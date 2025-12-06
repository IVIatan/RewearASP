using Microsoft.Data.SqlClient;
using RewearApi.BL;
using System;
using System.Collections.Generic;
using System.Data;

namespace RewearApi.DAL
{
    public class ClothingItemDAL : DBServices
    {
        private const string CON_STR_NAME = "RewearDB";

        private const string SP_GET_ALL = "GetAllClothingItems";
        private const string SP_GET_BY_ID = "GetClothingItemById";
        private const string SP_GET_BY_OWNER = "GetClothingItemsByOwnerUserId";
        private const string SP_GET_BY_STORE = "GetClothingItemsByStoreId";
        private const string SP_ADD = "AddClothingItem";
        private const string SP_UPDATE = "UpdateClothingItem";
        private const string SP_DELETE = "DeleteClothingItem";

        public List<ClothingItem> GetAllClothingItems()
        {
            List<ClothingItem> items = new List<ClothingItem>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    SqlCommand cmd = CreateCommand(SP_GET_ALL, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapClothingItem(reader));
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClothingItem? GetClothingItemById(int itemId)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId", itemId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_ID, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapClothingItem(reader);
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

        public List<ClothingItem> GetClothingItemsByOwnerUserId(int ownerUserId)
        {
            List<ClothingItem> items = new List<ClothingItem>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@OwnerUserId", ownerUserId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_OWNER, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapClothingItem(reader));
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClothingItem> GetClothingItemsByStoreId(int storeId)
        {
            List<ClothingItem> items = new List<ClothingItem>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@StoreId", storeId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_STORE, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapClothingItem(reader));
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddClothingItem(ClothingItem item)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@OwnerUserId",        item.OwnerUserId },
                        { "@StoreId",            (object?)item.StoreId ?? DBNull.Value },
                        { "@Title",              item.Title },
                        { "@Description",        (object?)item.Description ?? DBNull.Value },
                        { "@Category",           item.Category },
                        { "@Size",               item.Size },
                        { "@Condition",          item.Condition },
                        { "@Color",              item.Color },
                        { "@Price",              (object?)item.Price ?? DBNull.Value },
                        { "@Status",             item.Status },
                        { "@ExpirationDate",     (object?)item.ExpirationDate ?? DBNull.Value },
                        { "@DestinationStoreId", (object?)item.DestinationStoreId ?? DBNull.Value }
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

        public int UpdateClothingItem(ClothingItem item)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId",            item.ItemId },
                        { "@OwnerUserId",       item.OwnerUserId },
                        { "@StoreId",           (object?)item.StoreId ?? DBNull.Value },
                        { "@Title",             item.Title },
                        { "@Description",       (object?)item.Description ?? DBNull.Value },
                        { "@Category",          item.Category },
                        { "@Size",              item.Size },
                        { "@Condition",         item.Condition },
                        { "@Color",             item.Color },
                        { "@Price",             (object?)item.Price ?? DBNull.Value },
                        { "@Status",            item.Status },
                        { "@ExpirationDate",    (object?)item.ExpirationDate ?? DBNull.Value },
                        { "@DestinationStoreId", (object?)item.DestinationStoreId ?? DBNull.Value }
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

        public int DeleteClothingItem(int itemId)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId", itemId }
                    };

                    SqlCommand cmd = CreateCommand(SP_DELETE, con, paramDic);

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

        public int UpdateImage(int itemId, string imagePath)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    string sql = "UPDATE ClothingItems SET ImagePath = @ImagePath WHERE ItemId = @ItemId";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                        cmd.Parameters.AddWithValue("@ItemId", itemId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ClothingItem MapClothingItem(SqlDataReader reader)
        {
            ClothingItem c = new ClothingItem();

            c.ItemId = Convert.ToInt32(reader["ItemId"]);
            c.OwnerUserId = Convert.ToInt32(reader["OwnerUserId"]);
            c.OwnerFullName = reader["OwnerFullName"].ToString();

            c.StoreId = reader["StoreId"] == DBNull.Value
                ? null
                : Convert.ToInt32(reader["StoreId"]);

            c.StoreName = reader["StoreName"] == DBNull.Value
                ? null
                : reader["StoreName"].ToString();

            c.Title = reader["Title"].ToString()!;
            c.Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString();
            c.Category = reader["Category"].ToString()!;
            c.Size = reader["Size"].ToString()!;
            c.Condition = reader["Condition"].ToString()!;
            c.Color = reader["Color"].ToString()!;

            c.Price = reader["Price"] == DBNull.Value
                ? null
                : Convert.ToDecimal(reader["Price"]);

            c.Status = reader["Status"].ToString()!;

            c.ExpirationDate = reader["ExpirationDate"] == DBNull.Value
                ? null
                : Convert.ToDateTime(reader["ExpirationDate"]);

            c.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

            c.DestinationStoreId = reader["DestinationStoreId"] == DBNull.Value
                ? null
                : Convert.ToInt32(reader["DestinationStoreId"]);

            c.DestinationStoreName = reader["DestinationStoreName"] == DBNull.Value
                ? null
                : reader["DestinationStoreName"].ToString();

            return c;
        }
    }
}
