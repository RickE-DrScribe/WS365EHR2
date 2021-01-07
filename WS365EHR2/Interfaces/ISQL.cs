using System.Data;
using System.ServiceModel;
using WS365EHR.Models;

namespace WS365EHR.Interfaces
{
    /// <summary>
    /// Interface ISQL
    /// </summary>
    [ServiceContract]
    public interface ISQL
    {
        /// <summary>
        /// Wses the global execute non query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Int32.</returns>
        [OperationContract]
        int ws_GlobalExecuteNonQuery(string passKey, string username, string procName, SPParam[] paramList);

        /// <summary>
        /// Wses the global execute query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        [OperationContract]
        DataSet ws_GlobalExecuteQuery(string passKey, string username, string procName, SPParam[] paramList);

        /// <summary>
        /// Wses the global execute scalar.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        [OperationContract]
        int? ws_GlobalExecuteScalar(string passKey, string username, string procName, SPParam[] paramList);

        /// <summary>
        /// Wses the execute non query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Int32.</returns>
        [OperationContract]
        int ws_ExecuteNonQuery(string passKey, string dbName, string username, string procName, SPParam[] paramList);

        /// <summary>
        /// Wses the execute query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        [OperationContract]
        DataSet ws_ExecuteQuery(string passKey, string dbName, string username, string procName, SPParam[] paramList);

        /// <summary>
        /// Wses the execute scalar.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        [OperationContract]
        int? ws_ExecuteScalar(string passKey, string dbName, string username, string procName, SPParam[] paramList);

    }
}