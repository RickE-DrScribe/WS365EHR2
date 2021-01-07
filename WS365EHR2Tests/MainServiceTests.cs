using System.Collections.Generic;
using System.Data;
using LexiData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WS365EHR;
using WS365EHR.Models;


namespace WS365EHRTests
{
    /// <summary>
    /// Defines the <see cref="MainServiceTests" />
    /// </summary>
    [TestClass()]
    public class MainServiceTests
    {
        #region Methods

        /// <summary>
        /// The ws_ExecuteNonQueryTest
        /// </summary>
        [TestMethod()]
        public void ws_ExecuteNonQueryTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "0")};
            MainService ms = new MainService();
            int ds = ms.ws_ExecuteNonQuery("xy1000#dr", "xsql1.CertDB", "ricke.drs", "[UnitTest_NonQuery]", _paramList.ToArray());
            if (ds == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// The ws_ExecuteQueryTest
        /// </summary>
        [TestMethod()]
        public void ws_ExecuteQueryTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "0")};
            MainService ms = new MainService();
            DataSet ds = ms.ws_ExecuteQuery("xy1000#dr", "xsql1.CertDB", "ricke.drs", "[UnitTest_ExecuteQuery]", _paramList.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// The ws_ExecuteScalarTest
        /// </summary>
        [TestMethod()]
        public void ws_ExecuteScalarTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "7")};
            MainService ms = new MainService();
            int? ds = ms.ws_ExecuteScalar("xy1000#dr", "xsql1.CertDB", "ricke.drs", "[UnitTest_ScalarQuery]", _paramList.ToArray());
            if (ds != 7)
            {
                Assert.Fail("Invalid Data returned");
            }
        }

        /// <summary>
        /// The ws_GlobalExecuteNonQueryTest
        /// </summary>
        [TestMethod()]
        public void ws_GlobalExecuteNonQueryTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "0")};
            MainService ms = new MainService();
            int ds = ms.ws_GlobalExecuteNonQuery("xy1000#dr", "ricke.drs", "[UnitTest_GlobalNonQuery]", _paramList.ToArray());
            if (ds == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// The ws_GlobalExecuteQueryTest
        /// </summary>
        [TestMethod()]
        public void ws_GlobalExecuteQueryTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "0")};
            MainService ms = new MainService();
            DataSet ds = ms.ws_GlobalExecuteQuery("xy1000#dr", "ricke.drs", "[UnitTest_GlobalExecuteQuery]", _paramList.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// The ws_GlobalExecuteScalarTest
        /// </summary>
        [TestMethod()]
        public void ws_GlobalExecuteScalarTest()
        {
            List<SPParam> _paramList = new List<SPParam> {new SPParam("@Id", "5")};
            MainService ms = new MainService();
            int? ds = ms.ws_GlobalExecuteScalar("xy1000#dr", "ricke.drs", "[UnitTest_GlobalScalarQuery]", _paramList.ToArray());
            if (ds != 5)
            {
                Assert.Fail("Invalid Data returned");
            }
        }

        /// <summary>
        /// Defines the test method rx_GetCommonOrderResultsTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetCommonOrderResultsTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetCommonOrderResults("", 24958, 0, "xy1000#dr");
            if (ds.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        #endregion

        /// <summary>
        /// Defines the test method rx_GetDoseRouteAllTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetDoseRouteAllTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetDoseRouteAll("xy1000#dr");
            if (ds.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// Defines the test method rx_GetDrugAllergyInteractionsTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetDrugAllergyInteractionsTest()
        {
            MainService ms = new MainService();
            string[] gid = new string[]{"d07727"};
            int[] acid = new int[0];
            var ds = ms.rx_GetDrugAllergyInteractions( gid,acid,"d00003","xy1000#dr");
            if (ds.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

        /// <summary>
        /// Defines the test method rx_GetDrugDrugInteractionsTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetDrugDrugInteractionsTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetDrugDrugInteractions(1286,"d03826","xy1000#dr");
            if (ds.Count == 0)
            {
                Assert.Fail("No Data returned");
            }

            ds = ms.rx_GetDrugDrugInteractions(0,"d03826","xy1000#dr");
            if (ds != null)
            {
                if (ds.Count != 0)
                {
                    Assert.Fail("Data returned in error");
                }
            }
        }

        /// <summary>
        /// Defines the test method rx_GetGenericDrugFromProductIdTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetGenericDrugFromProductIdTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetGenericDrugFromProductId(24958, "xy1000#dr");
            if (ds == null)
            {
                Assert.Fail("No Generic Drug object returned");
            }
        }

        /// <summary>
        /// Defines the test method rx_GetGenericDrugProductsTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetGenericDrugProductsTest()
        {
            MainService ms = new MainService();
            DrugFilter df = new DrugFilter();
            df.IncludeBrandName = true;
            df.IncludeSynonyms = false;
            df.RxOTCStatus = RX_OTC_STATUS_CODE.BOTH;
            var ds = ms.rx_GetGenericDrugProducts("d07371", df, "xy1000#dr");
            if (ds != null)
            {
                if (ds.Count == 0)
                {
                    Assert.Fail("Data returned in error");
                }
            }
            else
            {
                Assert.Fail("No List object returned");
            }
        }

        /// <summary>
        /// Defines the test method rx_GetGenericProductTest.
        /// </summary>
        [TestMethod()]
        public void rx_GetGenericProductTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetGenericProduct(24958, "xy1000#dr");
            if (ds != null)
            {
                if (ds.GenericProductName.Trim() == "")
                {
                    Assert.Fail("Data returned in error");
                }
            }
            else
            {
                Assert.Fail("No object returned");
            }
        }

        [TestMethod()]
        public void rx_SearchGenericDrugTest()
        {
            MainService ms = new MainService();

            SearchCriteria sc = new SearchCriteria();
            sc.SearchType = SEARCHTYPE.Contains;
            sc.SearchMethod = SEARCHMETHOD.Normalized;

            DrugFilter df = new DrugFilter();
            df.IncludeBrandName = true;
            df.IncludeSynonyms = false;
            df.RxOTCStatus = RX_OTC_STATUS_CODE.BOTH;

            var ds = ms.rx_SearchGenericDrug("acet", sc, df, "xy1000#dr");
            if (ds != null)
            {
                if (ds.Count == 0)
                {
                    Assert.Fail("Data returned in error");
                }
            }
            else
            {
                Assert.Fail("No List object returned");
            }
        }

        [TestMethod()]
        public void rx_GetDrugInteractionsTest()
        {
            MainService ms = new MainService();
            var ds = ms.rx_GetDrugInteractions(13775,"losartan","xy1000#dr");
            if (ds.Count == 0)
            {
                Assert.Fail("No Data returned");
            }
        }

    }
}
