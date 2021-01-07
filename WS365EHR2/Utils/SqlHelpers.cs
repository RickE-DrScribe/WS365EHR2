using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using LexiData;

namespace WS365EHR.Utils
{
    /// <summary>
    /// Class SqlConn.
    /// </summary>
    public static class SqlHelpers
    {
        internal static readonly string LexidataConnectionString = ConfigurationManager.AppSettings["LexiDataConn"];

        internal static readonly string SqlConnectionStringSource = ConfigurationManager.AppSettings["SQLConn"];

        private const string ProviderInvariantName = "System.Data.SqlClient";


        #region Methods

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="dbConn">The database connection.</param>
        public static void CloseConnection(DbConnection dbConn)
        {
            if (dbConn != null)
            {
                if (dbConn.State != ConnectionState.Closed)
                {
                    dbConn.Close();
                }
                dbConn.Dispose();
            }
        }
        /// <summary>
        /// Get Lexidata Data Access Layer.
        /// </summary>
        /// <param name="dbConn">The dbConn<see cref="SqlConnection"/>.</param>
        /// <returns>The <see cref="GenericDAL" />.</returns>
        public static GenericDAL GetLexidataDAL(SqlConnection dbConn)
        {
            return new GenericDAL(CreateDbProviderFactory(), dbConn, ProviderInvariantName);
        }
        /// <summary>
        /// Creates the database provider factory.
        /// </summary>
        /// <returns>DbProviderFactory.</returns>
        private static DbProviderFactory CreateDbProviderFactory()
        {
            //Your DB specific information
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(ProviderInvariantName);
            if (dbFactory == null)
            {
                throw new Exception("Unable to create a DbFactory");
            }

            return dbFactory;
        }

        /// <summary>
        /// Gets the open SQL connection.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>SqlConnection.</returns>
        public static SqlConnection GetOpenSqlConnection(string dbName)
        {
            string sqlConnectionString = SqlConnectionStringSource;

            if (!string.IsNullOrEmpty(dbName))
            {

                if (dbName.Contains("."))
                {
                    string[] splitDbName = dbName.Split('.');

                    sqlConnectionString = sqlConnectionString.Replace("xsql1", splitDbName[0]);
                    sqlConnectionString = sqlConnectionString.Replace("DrScribeGlobal", splitDbName[1]);

                }
                else
                {
                    sqlConnectionString = sqlConnectionString.Replace("DrScribeGlobal", dbName);
                }
            }

            try
            {
                SqlConnection con = new SqlConnection(sqlConnectionString);
                con.Open();
                return con;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Opens the lexidata database connection.
        /// </summary>
        /// <returns>The <see cref="SqlConnection"/>.</returns>
        public static SqlConnection OpenLexidataConnection()
        {
            SqlConnection sc = new SqlConnection
            {
                ConnectionString = LexidataConnectionString
            };
            sc.Open();
            return sc;
        }

        #endregion
    }
}
