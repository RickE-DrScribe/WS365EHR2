using System.ServiceModel;

namespace WS365EHR.Interfaces
{
    /// <summary>
    /// Interface IFile
    /// </summary>
    [ServiceContract]
    public interface IFile
    {
        /// <summary>
        /// Uploads the file FTP.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="server">The server.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [OperationContract]
        bool UploadFileFTP(string passKey, string server, string username, string password, byte[] claim, string folder, string fileName);

        /// <summary>
        /// Wses the get file.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.Byte[].</returns>
        [OperationContract]
        byte[] ws_GetFile(string passKey, string username, string filePath);

        /// <summary>
        /// Wses the write file.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="fullFilePathAndName">Full name of the file path and.</param>
        /// <param name="image">The image.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [OperationContract]
        bool ws_WriteFile(string passKey, string username, string fullFilePathAndName, byte[] image);

    }
}