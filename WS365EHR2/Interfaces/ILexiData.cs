using System.Collections.Generic;
using System.ServiceModel;
using WS365EHR.Models;

namespace WS365EHR.Interfaces
{
    /// <summary>
    /// Interface ILexiData
    /// </summary>
    [ServiceContract]
    public interface ILexiData
    {
        /// <summary>
        /// Rxes the get common order results.
        /// </summary>
        /// <param name="drugId">The drug identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="localDataSetId">The local data set identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.CommonOrderResult&gt;.</returns>
        [OperationContract]
        List<LexiData.CommonOrderResult> rx_GetCommonOrderResults(string drugId, int productId, int localDataSetId, string passKey);

        /// <summary>
        /// Rxes the get dose route all.
        /// </summary>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.DoseRoute&gt;.</returns>
        [OperationContract]
        List<LexiData.DoseRoute> rx_GetDoseRouteAll(string passKey);

        /// <summary>
        /// Rxes the get drug allergy interactions.
        /// </summary>
        /// <param name="genericDrugIds">The generic drug ids.</param>
        /// <param name="allergyClassIds">The allergy class ids.</param>
        /// <param name="testDrugId">The test drug identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;DrugAllergyInterResult&gt;.</returns>
        [OperationContract]
        List<DrugAllergyInterResult> rx_GetDrugAllergyInteractions(string[] genericDrugIds, int[] allergyClassIds, string testDrugId, string passKey);

        /// <summary>
        /// Rxes the get drug drug interactions.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="genDrugId">The gen drug identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;DrugDrugInterResult&gt;.</returns>
        [OperationContract]
        List<DrugDrugInterResult> rx_GetDrugDrugInteractions(int productId, string genDrugId, string passKey);

        /// <summary>
        /// get drug interactions for a patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="genDrugId">The gen drug identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;DrugDrugInterResult&gt;.</returns>
        [OperationContract]
        List<DrugDrugInterResult> rx_GetDrugInteractions(int patientId, string genDrugId, string passKey);

        /// <summary>
        /// Rxes the get generic drug from product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>LexiData.GenericDrug.</returns>
        [OperationContract]
        LexiData.GenericDrug rx_GetGenericDrugFromProductId(int productId, string passKey);

        /// <summary>
        /// Rxes the get generic drug products.
        /// </summary>
        /// <param name="genericDrugId">The generic drug identifier.</param>
        /// <param name="df">The df.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.GenericProduct&gt;.</returns>
        [OperationContract]
        List<LexiData.GenericProduct> rx_GetGenericDrugProducts(string genericDrugId, LexiData.DrugFilter df, string passKey);

        /// <summary>
        /// Rxes the get generic product.
        /// </summary>
        /// <param name="genericProductId">The generic product identifier.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>LexiData.GenericProduct.</returns>
        [OperationContract]
        LexiData.GenericProduct rx_GetGenericProduct(int genericProductId, string passKey);

        /// <summary>
        /// Rxes the search generic drug.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="drugFilter">The drug filter.</param>
        /// <param name="passKey">The pass key.</param>
        /// <returns>List&lt;LexiData.GenericDrug&gt;.</returns>
        [OperationContract]
        List<LexiData.GenericDrug> rx_SearchGenericDrug(string searchText, LexiData.SearchCriteria searchCriteria, LexiData.DrugFilter drugFilter, string passKey);

    }
}