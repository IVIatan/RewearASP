using Microsoft.Data.SqlClient;
using RewearApi.BL;
using System;
using System.Collections.Generic;

namespace RewearApi.DAL
{
    public class StoreDAL : DBServices
    {
        private const string CON_STR_NAME = "RewearDB";

        private const string SP_GET_ALL = "GetAllStores";
        private const string SP_GET_BY_ID = "GetStoreById";
        private const string SP_ADD = "AddStore";
        private const string SP_UPDATE = "UpdateStore";


        public List<Store> GetAllStores()
        {
            List<Store> stores = new List<Store>();

            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    SqlCommand cmd = CreateCommand(SP_GET_ALL, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stores.Add(MapStore(reader));
                        }
                    }
                }

                return stores;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Store? GetStoreById(int storeId)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@StoreId", storeId }
                    };

                    SqlCommand cmd = CreateCommand(SP_GET_BY_ID, con, paramDic);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapStore(reader);
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

        public int AddStore(Store store)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@StoreName", store.StoreName },
                        { "@StoreType", store.StoreType },
                        { "@City",      store.City },
                        { "@Address",   (object?)store.Address ?? DBNull.Value },
                        { "@Phone",     (object?)store.Phone ?? DBNull.Value },
                        { "@Email",     (object?)store.Email ?? DBNull.Value },
                        { "@IsActive",  store.IsActive }
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

        public int UpdateStore(Store store)
        {
            try
            {
                using (SqlConnection con = Connect(CON_STR_NAME))
                {
                    var paramDic = new Dictionary<string, object>
                    {
                        { "@StoreId",   store.StoreId },
                        { "@StoreName", store.StoreName },
                        { "@StoreType", store.StoreType },
                        { "@City",      store.City },
                        { "@Address",   (object?)store.Address ?? DBNull.Value },
                        { "@Phone",     (object?)store.Phone ?? DBNull.Value },
                        { "@Email",     (object?)store.Email ?? DBNull.Value },
                        { "@IsActive",  store.IsActive }
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


        private Store MapStore(SqlDataReader reader)
        {
            Store s = new Store();

            s.StoreId = Convert.ToInt32(reader["StoreId"]);
            s.StoreName = reader["StoreName"].ToString()!;
            s.StoreType = reader["StoreType"].ToString()!;
            s.City = reader["City"].ToString()!;

            s.Address = reader["Address"] == DBNull.Value
                ? null
                : reader["Address"].ToString();

            s.Phone = reader["Phone"] == DBNull.Value
                ? null
                : reader["Phone"].ToString();

            s.Email = reader["Email"] == DBNull.Value
                ? null
                : reader["Email"].ToString();

            s.IsActive = Convert.ToBoolean(reader["IsActive"]);

            if (reader["CreatedAt"] != DBNull.Value)
            {
                s.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            }

            return s;
        }
    }
}
