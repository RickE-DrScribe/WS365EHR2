using System;
using System.Data;
using System.Data.SqlClient;
using WS365EHR.Models;

namespace WS365EHR.Utils
{
    /// <summary>
    /// Class SpParamHelpers.
    /// </summary>
    public static class SpParamHelpers
    {
        /// <summary>
        /// Checks the sp parameters.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="practiceName">Name of the practice.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        public static DataSet CheckSpParams(string procName, string practiceName, SPParam[] paramList)
        {
            SqlConnection con = null;
            SqlCommand sqlCmd = null;
            SqlDataReader rdr = null;

            try
            {
                con = SqlHelpers.GetOpenSqlConnection(practiceName);
                string sqlText = "select name from sysobjects where type='P' and name = '" +
                    procName.Trim().Replace("[", "").Replace("]", "") + "'";

                sqlCmd = new SqlCommand(sqlText, con) {CommandType = CommandType.Text, CommandTimeout = 120};

                rdr = sqlCmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();

                    return HandleExceptionHelper.HandleSqlException(new Exception("Stored Procedure Does Not Exist"), procName, paramList);
                }
                rdr.Close();

                sqlText = "select distinct s.name from  syscolumns s inner join systypes t on s.xtype = t.xtype " +
                    " where id = (select id from sysobjects where name = '" +
                    procName.Trim().Replace("[", "").Replace("]", "") + "')";

                sqlCmd = new SqlCommand(sqlText, con) {CommandType = CommandType.Text, CommandTimeout = 120};
                rdr = sqlCmd.ExecuteReader();
                while (rdr.Read())
                {
                    var pName = Convert.ToString(rdr.GetValue(0));
                    var pNameFound = false;
                    foreach (SPParam sp in paramList)
                    {
                        if (pName.Equals(sp.Name))
                        {
                            pNameFound = true;
                        }
                    }
                    if (!pNameFound)
                    {
                        rdr.Close();

                        return HandleExceptionHelper.HandleSqlException(new Exception("SP Parameter " + pName + " Missing"), procName, paramList);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                return HandleExceptionHelper.HandleGenericException("1", "Exception during CheckSPParams: " + ex.Message);
            }
            finally
            {
                sqlCmd?.Dispose();
                rdr?.Dispose();
                SqlHelpers.CloseConnection(con);
            }

            return null;
        }

    }
}