using System.ServiceModel;
using WS365EHR.Models;

namespace WS365EHR.Interfaces
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IMainService : ILexiData, ISQL, IFile
    {

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        string GetData(int value);

        /// <summary>
        /// Gets the data using data contract.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <returns>SPParam.</returns>
        [OperationContract]
        SPParam GetDataUsingDataContract(SPParam composite);

        /// <summary>
        /// Wses the decrypt string.
        /// </summary>
        /// <param name="toDecrypt">To decrypt.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        string ws_DecryptString(string toDecrypt);

        /// <summary>
        /// Wses the encrypt string.
        /// </summary>
        /// <param name="toEncrypt">To encrypt.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        string ws_EncryptString(string toEncrypt);

        // TODO: Add your service operations here
    }
}
 
