namespace RewearApi.DAL
{
    using global::RewearApi.DAL;
    using Microsoft.Data.SqlClient;
    using RewearApi.BL;
    using System;
    using System.Collections.Generic;

        public class UserDAL : DBServices
        {
            private const string CON_STR_NAME = "RewearDB";

            private const string SP_GET_ALL = "GetAllUsers";
            private const string SP_GET_BY_ID = "GetUserById";
            private const string SP_ADD = "AddUser";
            private const string SP_UPDATE = "UpdateUser";


            public List<User> GetAllUsers()
            {
                List<User> users = new List<User>();

                try
                {
                    using (SqlConnection con = Connect(CON_STR_NAME))
                    {
                        SqlCommand cmd = CreateCommand(SP_GET_ALL, con);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapUser(reader));
                            }
                        }
                    }

                    return users;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public User? GetUserById(int id)
            {
                try
                {
                    using (SqlConnection con = Connect(CON_STR_NAME))
                    {
                        var paramDic = new Dictionary<string, object>
                    {
                        { "@UserId", id }
                    };

                        SqlCommand cmd = CreateCommand(SP_GET_BY_ID, con, paramDic);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapUser(reader);
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


            public int AddUser(User user)
            {
                try
                {
                    using (SqlConnection con = Connect(CON_STR_NAME))
                    {
                        var paramDic = new Dictionary<string, object>
                    {
                        { "@FullName",    user.FullName },
                        { "@Email",       user.Email },
                        { "@PasswordHash", user.Password },
                        { "@Phone",       (object?)user.Phone ?? DBNull.Value },
                        { "@City",        user.City },
                        { "@UserType",    user.UserType }
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

            public int UpdateUser(User user)
            {
                try
                {
                    using (SqlConnection con = Connect(CON_STR_NAME))
                    {
                        var paramDic = new Dictionary<string, object>
                    {
                        { "@UserId",   user.UserId },
                        { "@FullName", user.FullName },
                        { "@Phone",    (object?)user.Phone ?? DBNull.Value },
                        { "@City",     user.City },
                        { "@UserType", user.UserType },
                        { "@IsActive", user.IsActive }
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


            private User MapUser(SqlDataReader reader)
            {
                User u = new User();

                u.UserId = Convert.ToInt32(reader["UserId"]);
                u.FullName = reader["FullName"].ToString()!;
                u.Email = reader["Email"].ToString()!;

                u.Phone = reader["Phone"] == DBNull.Value
                    ? null
                    : reader["Phone"].ToString();

                u.City = reader["City"].ToString()!;
                u.UserType = reader["UserType"].ToString()!;
                u.IsActive = Convert.ToBoolean(reader["IsActive"]);

                if (reader["CreatedAt"] != DBNull.Value)
                {
                    u.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                }

                return u;
            }
        }
}


