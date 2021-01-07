using DRSCommon.Core;
using DRSCommon.SQL;
using LexiData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Text;
using Tamir.SharpSsh;
using WS365EHR.Interfaces;
using WS365EHR.Models;
using WS365EHR.Utils;
using static DRSCommon.DrScribe.DRSModule;
using static WS365EHR.Utils.SqlHelpers;

namespace WS365EHR
{
    /// <summary>
    /// Defines the <see cref="MainService" />.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class MainService:IMainService
    {
        #region Methods



 
        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="value">The value<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetData(int value)
        {
            return $"You entered: {value}";
        }

        /// <summary>
        /// The GetDataUsingDataContract.
        /// </summary>
        /// <param name="composite">The composite<see cref="SPParam"/>.</param>
        /// <returns>The <see cref="SPParam"/>.</returns>
        public SPParam GetDataUsingDataContract(SPParam composite)
        {
            if (composite is null)
            {
                throw new ArgumentNullException(nameof(composite));
            }
            if (composite.Name != "")
            {
                composite.Name += " Name";
            }
            return composite;
        }


        /// <summary>
        /// Get common order results.
        /// </summary>
        /// <param name="drugId">The drug identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="localDataSetId">The local data set identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.CommonOrderResult&gt;.</returns>
        public List<CommonOrderResult> rx_GetCommonOrderResults(string drugId, int productId, int localDataSetId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                var result = myDal.GetCommonOrderResults(drugId, productId, localDataSetId);

                return result;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetCommonOrderResults");
                sb.AppendLine(drugId == null ? "DrugId is null" : "DrugId: " + drugId);
                sb.AppendLine(productId == 0 ? "productId is zero" : "productId: " + productId);
                sb.AppendLine(localDataSetId == 0 ? "localDataSetID is zero" : "localDataSetID: " + localDataSetId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get dose route all.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.DoseRoute&gt;.</returns>
        public List<DoseRoute> rx_GetDoseRouteAll(string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                var route = myDal.GetDoseRouteAll();

                return route;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetDoseRouteAll");
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get drug allergy interactions.
        /// </summary>
        /// <param name="genericDrugIds">The generic drug ids.</param>
        /// <param name="allergyClassIds">The allergy class ids.</param>
        /// <param name="testDrugId">The test drug identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;DrugAllergyInterResult&gt;.</returns>
        public List<DrugAllergyInterResult> rx_GetDrugAllergyInteractions(string[] genericDrugIds, int[] allergyClassIds, string testDrugId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;
            ScreeningContext sc = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                sc = new ScreeningContext();

                foreach (string gdi in genericDrugIds)
                {
                    GenericDrug drugAllergen = myDal.GetGenericDrug(gdi);
                    sc.Allergies.Add(drugAllergen);
                }

                AllergyClass classAllergen = myDal.GetAllergyClass(3);
                sc.Allergies.Add(classAllergen);

                GenericDrug drug1 = myDal.GetGenericDrug(testDrugId);
                sc.Drugs.Add(drug1);


                List<DrugAllergyResult> results = myDal.GetDrugAllergyInteractions(sc, false);

                List<DrugAllergyInterResult> r = new List<DrugAllergyInterResult>();

                foreach (DrugAllergyResult ir in results)
                {
                    DrugAllergyInterResult newR = new DrugAllergyInterResult
                    {
                        Drug1 = { GenDrugId = ir.Drug.GenDrugID, GenericName = ir.Drug.GenericName },
                        Allergen1 =
                        {
                            AllergySeverity = ir.Allergen.AllergySeverity,
                            ConceptType = ir.Allergen.ConceptType.ToString(),
                            Reaction = ir.Allergen.Reaction
                        },
                        AllergyMessage = ir.AllergyMessage,
                        AllergySeverity = ir.AllergySeverity,
                        AllergyXrGroupDescription = ir.AllergyXRGroupDescription,
                        AllergyXrGroupId = ir.AllergyXRGroupID,
                        ClassDescription = ir.ClassDescription,
                        ClassDescriptionPlural = ir.ClassDescriptionPlural,
                        ClassId = ir.ClassID,
                        MatchTypeDescription = ir.MatchTypeDescription,
                        MatchTypeId = ir.MatchTypeID,
                        Reaction = ir.Reaction
                    };

                    r.Add(newR);
                }

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetDrugAllergyInteractions");
                if (genericDrugIds == null)
                {
                    sb.AppendLine("genericDrugIds array is null");
                }
                else
                {
                    foreach (string gdi in genericDrugIds)
                    {
                        sb.AppendLine(gdi == null ? "genericDrugId is null" : "genericDrugId: " + gdi);
                    }
                }
                if (allergyClassIds == null)
                {
                    sb.AppendLine("allergyClassIds array is null");
                }
                else
                {
                    foreach (int adi in allergyClassIds)
                    {
                        sb.AppendLine(adi == 0 ? "allergyClassId is zero" : "allergyClassId: " + adi);
                    }
                }
                sb.AppendLine(testDrugId == null ? "testDrugId is null" : "DrugId: " + testDrugId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                sc.Destroy();
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get drug 2 drug interactions.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="genDrugId">The gen drug identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;DrugDrugInterResult&gt;.</returns>
        public List<DrugDrugInterResult> rx_GetDrugDrugInteractions(int productId, string genDrugId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            if (productId == 0)
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                ScreeningContext screen = new ScreeningContext();

                GenericDrug gd = myDal.GetGenericDrug((myDal.GetGenericProduct(productId)).GenDrugID);
                if (gd != null)
                {
                    screen.Drugs.Add(gd);
                }

                GenericDrug newGd = myDal.GetGenericDrug(genDrugId);
                if (gd != null)
                {
                    screen.Drugs.Add(newGd);
                }

                var drugInteractions = myDal.GetDrugDrugInteractions(screen, false, 0);

                List<DrugDrugInterResult> r = new List<DrugDrugInterResult>();

                foreach (DrugDrugInteractionResult ir in drugInteractions)
                {
                    DrugDrugInterResult newR = new DrugDrugInterResult
                    {
                        Drug1 = { GenDrugId = ir.Drug1.GenDrugID, GenericName = ir.Drug1.GenericName },
                        Drug2 = { GenDrugId = ir.Drug2.GenDrugID, GenericName = ir.Drug2.GenericName },
                        InteractionDescription = ir.InteractionDescription,
                        SeverityDescription = ir.SeverityDescription
                    };

                    r.Add(newR);
                }

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetDrugDrugInteractions");
                sb.AppendLine("ProductId: " + productId);
                sb.AppendLine(genDrugId == null ? "genDrugId is null" : "genDrugId: " + genDrugId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// The rx_GetDrugInteractions.
        /// </summary>
        /// <param name="patientId">The patientId<see cref="int"/>.</param>
        /// <param name="genDrugId">The genDrugId<see cref="string"/>.</param>
        /// <param name="passKey">The passKey<see cref="string"/>.</param>
        /// <returns>The <see cref="List{DrugDrugInterResult}"/>.</returns>
        public List<DrugDrugInterResult> rx_GetDrugInteractions(int patientId, string genDrugId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            if (patientId == 0)
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                ScreeningContext interactionScreening = new ScreeningContext();



                // add existing drugs
                SqlCommand cmd = XSql1.CreateStoredProcedureCommand("rx_SelectPatientInteractionData");
                cmd.AddParameter("@PatientId", patientId);
                DataSet interactDs = XSql1.GetDataSet(cmd);

                foreach (DataRow row in interactDs.Tables[0].Rows)
                {
                    int productId = row["RootProductId"].ToString().ToInt(0);

                    if (productId > 0)
                    {
                        GenericDrug gd = myDal.GetGenericDrug((myDal.GetGenericProduct(productId)).GenDrugID);
                        if (gd != null)
                        {
                            interactionScreening.Drugs.Add(gd);
                        }
                    }
                }
                // Add new drug
                GenericDrug newGd = myDal.GetGenericDrug(genDrugId);
                if (newGd != null)
                {
                    interactionScreening.Drugs.Add(newGd);
                }

                var drugInteractions = myDal.GetDrugDrugInteractions(interactionScreening, false, 0);

                List<DrugDrugInterResult> ddirList = new List<DrugDrugInterResult>();

                foreach (DrugDrugInteractionResult ddir in drugInteractions)
                {
                    DrugDrugInterResult newDdir = new DrugDrugInterResult
                    {
                        Drug1 = { GenDrugId = ddir.Drug1.GenDrugID, GenericName = ddir.Drug1.GenericName },
                        Drug2 = { GenDrugId = ddir.Drug2.GenDrugID, GenericName = ddir.Drug2.GenericName },
                        InteractionDescription = ddir.InteractionDescription,
                        SeverityDescription = ddir.SeverityDescription
                    };

                    ddirList.Add(newDdir);
                }

                return ddirList;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetDrugInteractions");
                sb.AppendLine("PatientId: " + patientId);
                sb.AppendLine(genDrugId == null ? "genDrugId is null" : "genDrugId: " + genDrugId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get generic drug from product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>LexiData.GenericDrug.</returns>
        public GenericDrug rx_GetGenericDrugFromProductId(int productId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;
            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                GenericDrug gd = myDal.GetGenericDrug((myDal.GetGenericProduct(productId)).GenDrugID);

                return gd;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetGenericDrugFromProductId");
                sb.AppendLine(productId == 0 ? "ProductId is null" : "ProductId: " + productId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get generic drug products.
        /// </summary>
        /// <param name="genericDrugId">The generic drug identifier.</param>
        /// <param name="df">The df.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.GenericProduct&gt;.</returns>
        public List<GenericProduct> rx_GetGenericDrugProducts(string genericDrugId, DrugFilter df, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                var result = myDal.GetGenericDrugProducts(genericDrugId, df);

                return result;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetGenericDrugProducts");
                sb.AppendLine(genericDrugId == null ? "genericDrugId is null" : "genericDrugId: " + genericDrugId);
                sb.AppendLine(df == null ? "df is null" : "df: " + df);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get generic product.
        /// </summary>
        /// <param name="genericProductId">The generic product identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>LexiData.GenericProduct.</returns>
        public GenericProduct rx_GetGenericProduct(int genericProductId, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                var genericProduct = myDal.GetGenericProduct(genericProductId);

                return genericProduct;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_GetGenericProduct");
                sb.AppendLine(genericProductId == 0 ? "genericProductId is null" : "genericProductId: " + genericProductId);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Search generic drug.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="drugFilter">The drug filter.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.GenericDrug&gt;.</returns>
        public List<GenericDrug> rx_SearchGenericDrug(string searchText, SearchCriteria searchCriteria, DrugFilter drugFilter, string passKey)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            SqlConnection dbConn = new SqlConnection();
            GenericDAL myDal = null;

            try
            {
                dbConn = OpenLexidataConnection();
                myDal = GetLexidataDAL(dbConn);

                var genericDrug = myDal.SearchGenericDrug(searchText, searchCriteria, drugFilter);

                return genericDrug;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Method: rx_SearchGenericDrug");
                sb.AppendLine(searchText == null ? "searchText is null" : "searchText: " + searchText);
                sb.AppendLine(searchCriteria == null ? "searchCriteria is null" : "searchCriteria: " + searchCriteria);
                sb.AppendLine(drugFilter == null ? "drugFilter is null" : "drugFilter: " + drugFilter.RxOTCStatus);
                sb.AppendLine("");
                sb.AppendLine(e.ExceptionToString());
                WriteEventLogEntry(sb.ToString());
                sb.Clear();
                sb.Destroy();
                throw;
            }
            finally
            {
                myDal.Destroy();
                CloseConnection(dbConn);
            }
        }

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
        /// <returns>The <see cref="bool"/>.</returns>
        public bool UploadFileFTP(string passKey, string server, string username, string password, byte[] claim, string folder, string fileName)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return false;
            }

            string tempDir = "C:\\DrScribe\\GatewayEDI\\" + username + "\\claims\\";
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            try
            {
                var arraySize = claim.GetUpperBound(0);

                try
                {
                    if (File.Exists(tempDir + "\\" + fileName.Replace(".tmp", ".txt")))
                    {
                        File.Delete(tempDir + "\\" + fileName.Replace(".tmp", ".txt"));
                    }
                }
                catch (Exception e)
                {
                    WriteEventLogEntry(e);
                }

                FileStream fs = new FileStream(tempDir + "\\" + fileName.Replace(".tmp", ".txt"), FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(claim, 0, arraySize);
                fs.Close();

                SshTransferProtocolBase sshCp = new Sftp(server, username);
                sshCp.Password = password;

                sshCp.Connect();

                string serverFile = fileName.Replace(".tmp", ".txt");

                if (folder != string.Empty)
                {
                    serverFile = "/" + folder + "/" + serverFile;
                }

                sshCp.Put(tempDir + "\\" + fileName.Replace(".tmp", ".txt"), serverFile);

                sshCp.Close();
                sshCp.Destroy();

                File.Delete(tempDir + fileName.Replace(".tmp", ".txt"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the event log entry.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="logType">Type of the log.</param>
        private static void WriteEventLogEntry(Exception ex, EventLogEntryType logType = EventLogEntryType.Error)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine("");
            sb.AppendLine("StackTrace: " + ex.StackTrace);

            WriteEventLogEntry(sb.ToString(), logType);
            sb.Clear();
            sb.Destroy();
        }

        /// <summary>
        /// Writes the event log entry.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <param name="logType">Type of the log.</param>
        private static void WriteEventLogEntry(Exception ex, string storedProcName, SPParam[] paramList, EventLogEntryType logType = EventLogEntryType.Error)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SP: " + storedProcName);
            foreach (SPParam p in paramList)
            {
                if (p.Value == null)
                {
                    sb.AppendLine("SPParam: " + p.Name + " = null");
                }
                else
                {
                    sb.AppendLine("SPParam: " + p.Name + " = " + p.Value);
                }
            }
            sb.AppendLine("");
            sb.AppendLine(ex.ExceptionToString());

            WriteEventLogEntry(sb.ToString(), logType);
            sb.Clear();
            sb.Destroy();
        }

        /// <summary>
        /// Writes the event log entry.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        private static void WriteEventLogEntry(string message, EventLogEntryType logType = EventLogEntryType.Error)
        {
            // Create an instance of EventLog
            EventLog eventLog = new EventLog();

            // Check if the event source exists. If not create it.
            if (!EventLog.SourceExists("WS365HR"))
            {
                EventLog.CreateEventSource("WS365HR", "Application");
            }

            // Set the source name for writing log entries.
            eventLog.Source = "WS365HR";

            // Write an entry to the event log.
            eventLog.WriteEntry(message, logType, 365);

            // Close the Event Log
            eventLog.Close();
        }

        /// <summary>
        /// Decrypt string.
        /// </summary>
        /// <param name="toDecrypt">To decrypt.</param>
        /// <returns>System.String.</returns>
        public string ws_DecryptString(string toDecrypt)
        {
            try
            {
                return ValidationAndEncryptDecrypt.DecryptString(toDecrypt);
            }
            catch (Exception ex)
            {
                WriteEventLogEntry(ex);
                return null;
            }
        }

        /// <summary>
        /// Encrypt string.
        /// </summary>
        /// <param name="toEncrypt">To encrypt.</param>
        /// <returns>System.String.</returns>
        public string ws_EncryptString(string toEncrypt)
        {
            try
            {
                return ValidationAndEncryptDecrypt.EncryptString(toEncrypt);
            }
            catch (Exception ex)
            {
                WriteEventLogEntry(ex);
                return null;
            }
        }

        /// <summary>
        /// Execute non query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Int32.</returns>
        public int ws_ExecuteNonQuery(string passKey, string dbName, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return 0;
            }

            if (SpParamHelpers.CheckSpParams(procName, dbName, paramList) != null)
            {
                return -10;
            }

            SqlConnection dbConn = null;
            SqlCommand sqlCmd = null;

            try
            {
                dbConn = GetOpenSqlConnection(dbName);

                sqlCmd = new SqlCommand(procName, dbConn) {CommandType = CommandType.StoredProcedure};

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }

                return sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return -11;
            }
            finally
            {
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        public DataSet ws_ExecuteQuery(string passKey, string dbName, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            DataSet dsCheck = SpParamHelpers.CheckSpParams(procName, dbName, paramList);

            if (dsCheck != null)
            {
                return dsCheck;
            }

            SqlConnection dbConn = null;
            SqlCommand sqlCmd = null;
            SqlDataAdapter sqlAdapter = null;
            DataSet ds = null;

            try
            {
                dbConn = GetOpenSqlConnection(dbName);
                sqlCmd = new SqlCommand(procName, dbConn)
                {
                    CommandType = CommandType.StoredProcedure, CommandTimeout = 0
                };

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }

                ds = new DataSet();
                sqlAdapter = new SqlDataAdapter {SelectCommand = sqlCmd};
                sqlAdapter.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return HandleExceptionHelper.HandleSqlException(e, procName, paramList);
            }
            finally
            {
                ds?.Dispose();
                sqlAdapter?.Dispose();
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Execute scalar.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public int? ws_ExecuteScalar(string passKey, string dbName, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return 0;
            }

            SqlConnection dbConn = null;
            SqlCommand sqlCmd = null;

            try
            {
                dbConn = GetOpenSqlConnection(dbName);
                sqlCmd = new SqlCommand(procName, dbConn) {CommandType = CommandType.StoredProcedure};

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }
                return Convert.ToInt32(sqlCmd.ExecuteScalar());
                //}
            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return null;
            }
            finally
            {
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] ws_GetFile(string passKey, string username, string filePath)
        {
            byte[] blobData;

            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                blobData = br.ReadBytes((int) fs.Length);

                br.Close();
                fs.Close();
            }
            catch
            {
                return null;
            }

            return blobData;
        }

        /// <summary>
        /// Global execute non query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Int32.</returns>
        public int ws_GlobalExecuteNonQuery(string passKey, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return 0;
            }

            if (SpParamHelpers.CheckSpParams(procName, "", paramList) != null)
            {
                return -1;
            }

            SqlConnection dbConn = new SqlConnection();
            SqlCommand sqlCmd = null;

            try
            {
                dbConn = GetOpenSqlConnection("");

                sqlCmd = new SqlCommand(procName, dbConn) {CommandType = CommandType.StoredProcedure};

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }

                return sqlCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return -1;
            }
            finally
            {
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Global execute query.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>DataSet.</returns>
        public DataSet ws_GlobalExecuteQuery(string passKey, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return null;
            }

            DataSet dsCheck = SpParamHelpers.CheckSpParams(procName, "", paramList);

            if (dsCheck != null)
            {
                return dsCheck;
            }

            SqlConnection dbConn = null;
            SqlCommand sqlCmd = null;
            SqlDataAdapter sqlAdapter = null;
            DataSet ds = null;

            try
            {
                dbConn = GetOpenSqlConnection("");

                sqlCmd = new SqlCommand(procName, dbConn)
                {
                    CommandType = CommandType.StoredProcedure, CommandTimeout = 0
                };

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }

                ds = new DataSet();
                sqlAdapter = new SqlDataAdapter {SelectCommand = sqlCmd};
                sqlAdapter.Fill(ds);
                return ds;
                //}
            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return HandleExceptionHelper.HandleSqlException(e, procName, paramList);
            }
            finally
            {
                ds?.Dispose();
                sqlAdapter?.Dispose();
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Global execute scalar.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public int? ws_GlobalExecuteScalar(string passKey, string username, string procName, SPParam[] paramList)
        {
            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return 0;
            }

            SqlConnection dbConn = null;
            SqlCommand sqlCmd = null;

            try
            {
                dbConn = GetOpenSqlConnection("");

                sqlCmd = new SqlCommand(procName, dbConn) {CommandType = CommandType.StoredProcedure};

                foreach (SPParam p in paramList)
                {
                    SqlParameter param = sqlCmd.Parameters.AddWithValue(p.Name, p.Value);
                    if (p.Value == null)
                    {
                        param.IsNullable = true;
                        param.Value = DBNull.Value;
                    }
                }
                return Convert.ToInt32(sqlCmd.ExecuteScalar());
                //}
            }
            catch (Exception e)
            {
                WriteEventLogEntry(e, procName, paramList);
                return null;
            }
            finally
            {
                sqlCmd?.Dispose();
                CloseConnection(dbConn);
            }
        }

        /// <summary>
        /// Write file.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <param name="username">The username.</param>
        /// <param name="fullFilePathAndName">Full name of the file path and.</param>
        /// <param name="image">The image.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool ws_WriteFile(string passKey, string username, string fullFilePathAndName, byte[] image)
        {
            var arraySize = image.GetUpperBound(0);

            if (!ValidationAndEncryptDecrypt.ValidateKey(passKey))
            {
                return false;
            }

            FileStream fs = new FileStream(fullFilePathAndName, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(image, 0, arraySize + 1);
            fs.Close();
            return true;
        }

        #endregion
    }
}
