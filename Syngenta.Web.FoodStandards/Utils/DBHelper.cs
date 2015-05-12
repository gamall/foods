using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Syngenta.Web.FoodStandards.Utils
{
    public class DBHelper
    {
        
        private string sql_connection = ConfigurationManager.ConnectionStrings["SyngentaConnectionString"].ToString();

        public DateTime LastLoginDate(int userId)
        {
            string lastLoginDate = "";

            using (SqlConnection sc = new SqlConnection(sql_connection))
            {
                sc.Open();

                using (SqlCommand cmd = new SqlCommand("GetUserLastLoginDateByUserId", sc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userid", userId));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastLoginDate = reader["LastLoginDate"].ToString();
                        }
                    }
                }
            }

            return Convert.ToDateTime(lastLoginDate);
            
        }
        
        
        
        

        
        
        
    }
}