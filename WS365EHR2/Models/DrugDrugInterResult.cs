using System.Runtime.Serialization;

namespace WS365EHR.Models
{
    /// <summary>
    /// Class DrugDrugInterResult.
    /// </summary>
    [DataContract]
    public class DrugDrugInterResult
    {
        #region Constructors / Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DrugDrugInterResult"/> class.
        /// </summary>
        public DrugDrugInterResult() {
            Drug1 = new Drug();
            Drug2 = new Drug();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugDrugInterResult"/> class.
        /// </summary>
        /// <param name="drug1">The drug1.</param>
        /// <param name="drug2">The drug2.</param>
        /// <param name="severityDescription">The severity description.</param>
        /// <param name="interactionDescription">The interaction description.</param>
        public DrugDrugInterResult(Drug drug1, Drug drug2, string severityDescription, string interactionDescription)
        {
            Drug1 = drug1;
            Drug2 = drug2;
            SeverityDescription = severityDescription;
            InteractionDescription = interactionDescription;
        }
        #endregion

        #region Properties
        [DataMember(IsRequired = false)]
        public Drug Drug1 { get; set; }

        [DataMember(IsRequired = false)]
        public Drug Drug2 { get; set; }

        [DataMember(IsRequired = false)]
        public string SeverityDescription { get; set; }

        [DataMember(IsRequired = false)]
        public string InteractionDescription { get; set; }

        #endregion
    }
}