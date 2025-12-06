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
        private const string SP_DELETE = "DeleteClothingItem";
        private const string SP_UPDATE_IMAGE = "UpdateClothingItemImage";

        public List<ClothingItem> GetAll()
        {
            var list = new List<ClothingItem>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    SqlCommand cmd = CreateCommand(SP_GET_ALL, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapClothingItem(reader));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClothingItem? GetById(int id)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId", id }
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

        public List<ClothingItem> GetByOwner(int ownerUserId)
        {
            var list = new List<ClothingItem>();

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
                            list.Add(MapClothingItem(reader));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClothingItem> GetByStore(int storeId)
        {
            var list = new List<ClothingItem>();

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
                            list.Add(MapClothingItem(reader));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Add(ClothingItem item)
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
                        { "@DestinationStoreId", (object?)item.DestinationStoreId ?? DBNull.Value },
                        { "@ImagePath",          (object?)item.ImagePath ?? DBNull.Value }
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

        public int Update(ClothingItem item)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId",             item.ItemId },
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
                        { "@DestinationStoreId", (object?)item.DestinationStoreId ?? DBNull.Value },
                        { "@ImagePath",          (object?)item.ImagePath ?? DBNull.Value }
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

        public int Delete(int itemId)
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
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@ItemId",    itemId },
                        { "@ImagePath", imagePath }
                    };

                    SqlCommand cmd = CreateCommand(SP_UPDATE_IMAGE, con, paramDic);

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
            var item = new ClothingItem();

            item.ItemId = Convert.ToInt32(reader["ItemId"]);
            item.OwnerUserId = Convert.ToInt32(reader["OwnerUserId"]);

            if (reader["StoreId"] != DBNull.Value)
                item.StoreId = Convert.ToInt32(reader["StoreId"]);

            if (reader["DestinationStoreId"] != DBNull.Value)
                item.DestinationStoreId = Convert.ToInt32(reader["DestinationStoreId"]);

            item.Title = reader["Title"].ToString()!;
            item.Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString();
            item.Category = reader["Category"].ToString()!;
            item.Size = reader["Size"].ToString()!;
            item.Condition = reader["Condition"].ToString()!;
            item.Color = reader["color"].ToString()!;

            item.Price = reader["Price"] == DBNull.Value
                ? (decimal?)null
                : Convert.ToDecimal(reader["Price"]);

            item.Status = reader["Status"].ToString()!;

            item.ExpirationDate = reader["ExpirationDate"] == DBNull.Value
                ? (DateTime?)null
                : Convert.ToDateTime(reader["ExpirationDate"]);

            item.CreatedAt = reader["CreatedAt"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["CreatedAt"]);

            item.OwnerFullName = reader["OwnerFullName"] == DBNull.Value
                ? null
                : reader["OwnerFullName"].ToString();

            item.StoreName = reader["StoreName"] == DBNull.Value
                ? null
                : reader["StoreName"].ToString();

            item.DestinationStoreName = reader["DestinationStoreName"] == DBNull.Value
                ? null
                : reader["DestinationStoreName"].ToString();

            item.ImagePath = reader["ImagePath"] == DBNull.Value
                ? null
                : reader["ImagePath"].ToString();

            return item;
        }
    }
}
