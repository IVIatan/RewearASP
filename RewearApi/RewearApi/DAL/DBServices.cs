using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace RewearApi.DAL
{
    public class DBServices
    {
        // פונקציה לפתיחת חיבור למסד הנתונים על פי Connection String מה-appsettings.json
        protected SqlConnection Connect(string conStrName)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")   // קורא את הקובץ עם ההגדרות
                .Build();

            string connectionString = configuration.GetConnectionString(conStrName);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();                           // כאן נפתח החיבור בפועל ל-SQL Server
            return con;
        }

        // פונקציה שיוצרת SqlCommand שמריץ Stored Procedure + מוסיפה לו פרמטרים
        protected SqlCommand CreateCommand(string spName,
                                           SqlConnection con,
                                           Dictionary<string, object>? paramDic = null)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure; // אומר לפקודה שאנחנו מריצים SP

            if (paramDic != null)
            {
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return cmd;
        }
    }
}

