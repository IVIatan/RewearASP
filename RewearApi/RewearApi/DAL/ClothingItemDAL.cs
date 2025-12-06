using Microsoft.Data.SqlClient;
using RewearApi.BL;
using System;
using System.Collections.Generic;

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

        private ClothingItem MapClothingItem(SqlDataReader reader)
        {
            ClothingItem item = new ClothingItem();

            item.ItemId = Convert.ToInt32(reader["ItemId"]);
            item.OwnerUserId = Convert.ToInt32(reader["OwnerUserId"]);
            item.StoreId = reader["StoreId"] == DBNull.Value ? (int?)null    : Convert.ToInt32(reader["StoreId"]);
            item.Title = reader["Title"].ToString()!;
            item.Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString();
            item.Category = reader["Category"].ToString()!;
            item.Size = reader["Size"].ToString()!;
            item.Condition = reader["Condition"].ToString()!;
            item.Color = reader["color"].ToString()!;   
            item.Price = reader["Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Price"]);
            item.Status = reader["Status"].ToString()!;
            item.ExpirationDate = reader["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ExpirationDate"]);
            item.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            item.DestinationStoreId = reader["DestinationStoreId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["DestinationStoreId"]);

            try
            {
                int ordOwnerFullName = reader.GetOrdinal("OwnerFullName");
                if (!reader.IsDBNull(ordOwnerFullName))
                {
                    item.OwnerFullName = reader.GetString(ordOwnerFullName);
                }
            }
            catch (IndexOutOfRangeException)
            {
                
            }

            try
            {
                int ordStoreName = reader.GetOrdinal("StoreName");
                if (!reader.IsDBNull(ordStoreName))
                {
                    item.StoreName = reader.GetString(ordStoreName);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            try
            {
                int ordDestStoreName = reader.GetOrdinal("DestinationStoreName");
                if (!reader.IsDBNull(ordDestStoreName))
                {
                    item.DestinationStoreName = reader.GetString(ordDestStoreName);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            return item;
        }
    }
}
