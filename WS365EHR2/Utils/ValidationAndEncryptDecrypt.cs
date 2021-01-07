using System;
using Dpapi;
using System.Text;

namespace WS365EHR.Utils
{
    /// <summary>
    /// Class ValidationAndEncryptDecrypt.
    /// </summary>
    public static class ValidationAndEncryptDecrypt
    {
        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidateKey(string passKey)
        {
            if (!passKey.Equals("xy1000#dr"))
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="inString">The in string.</param>
        /// <returns>System.String.</returns>
        public static string EncryptString(string inString)
        {
            string results = "";
            DataProtector dp = new DataProtector(Store.MachineStore);
            byte[] dataToEncrypt = Encoding.Unicode.GetBytes(inString);

            try
            {
                results = Convert.ToBase64String(dp.Encrypt(dataToEncrypt));
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("DrSched.asmx", "encryptString Exception: " + ex.Message);
            }
            return results;
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="inString">The in string.</param>
        /// <returns>System.String.</returns>
        public static string DecryptString(string inString)
        {
            System.Diagnostics.EventLog.WriteEntry("DrSched.asmx decryptString inString=", inString);
            string results = "";
            DataProtector dp = new DataProtector(Store.MachineStore);
            byte[] dataToDecrypt = Convert.FromBase64String(inString);

            try
            {
                results = Encoding.Unicode.GetString(dp.Decrypt(dataToDecrypt));
                System.Diagnostics.EventLog.WriteEntry("DrSched.asmx decryptString results=", results);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("DrSched.asmx", "Exception in decryptString " + ex.Message);
            }
            return results;
        }
    }
}