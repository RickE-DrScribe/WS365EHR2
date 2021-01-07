using System;
using System.Data;
using WS365EHR.Models;

namespace WS365EHR.Utils
{
    /// <summary>
    /// Class HandleExceptionHelper.
    /// </summary>
    public static class HandleExceptionHelper
    {
        /// <summary>
        /// Handles the SQL exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="wsName">Name of the ws.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        public static DataSet HandleSqlException(Exception ex, string wsName, SPParam[] paramList)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            dt.Columns.Add("WebServiceException");
            dt.Columns.Add("ProcedureName");
            dt.Columns.Add("Params");

            DataRow row = dt.NewRow();
            row[0] = ex.Message;
            row[1] = wsName;

            string strParamList = string.Empty;

            foreach (SPParam sp in paramList)
            {
                strParamList += strParamList == "" ? sp.Value.ToString() : "," + sp.Value;
            }

            string[] arr = strParamList.Split(',');
            bool isEmpty = string.Join("", arr).Trim() == string.Empty;

            row[2] = isEmpty ? "" : strParamList;

            dt.Rows.Add(row);
            ds.Tables.Add(dt);

            return ds;
        }

        /// <summary>
        /// Handles the generic exception.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>DataSet.</returns>
        public static DataSet HandleGenericException(string statusCode, string message)
        {
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("intStatusCode");
            dt.Columns.Add("strStausMessage");

            DataRow row = dt.NewRow();
            row[0] = statusCode;
            row[1] = message;

            dt.Rows.Add(row);
            ds.Tables.Add(dt);

            return ds;
        }
    }
}