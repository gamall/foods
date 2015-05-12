using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Syngenta.Web.FoodStandards
{
    public class Config
    {
        public static string ConnectionStringName
        {
            get 
            {
                if (ConfigurationManager.AppSettings["SyngentaConnectionString"] != null)
                {
                    return ConfigurationManager.AppSettings["SyngentaConnectionString"].ToString();
                }
                return "SyngentaConnectionString";
            }
        }

        public static string UsersTableName
        {
            get
            {
                if (ConfigurationManager.AppSettings["UsersTableName"] != null)
                {
                    return ConfigurationManager.AppSettings["UsersTableName"].ToString();
                }
                return "Users";
            }
        }
       
        public static string UsersPrimaryKeyColumnName
        {
            get
            {
                if (ConfigurationManager.AppSettings["UsersPrimaryKeyColumnName"] != null)
                {
                    return ConfigurationManager.AppSettings["UsersPrimaryKeyColumnName"].ToString();
                }
                return "Id";
            }
        }

        public static string UsersUserNameColumnName
        {
            get
            {
                if (ConfigurationManager.AppSettings["UsersUserNameColumnName"] != null)
                {
                    return ConfigurationManager.AppSettings["UsersUserNameColumnName"].ToString();
                }
                return "Email";
            }
        }

        public static string UsersSurname
        {
            get { return "Surname"; } 
        }

        public static string UsersEmail
        {
            get { return "Email"; }
        }
    }
}