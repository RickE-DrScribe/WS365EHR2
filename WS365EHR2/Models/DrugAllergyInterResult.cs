using System.Runtime.Serialization;

namespace WS365EHR.Models
{
    /// <summary>
    /// Class DrugAllergyInterResult.
    /// </summary>
    [DataContract]
    public class DrugAllergyInterResult
    {
        #region Constructors / Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DrugAllergyInterResult"/> class.
        /// </summary>
        public DrugAllergyInterResult() {
            Drug1 = new Drug();
            Allergen1 = new Allergen();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugAllergyInterResult"/> class.
        /// </summary>
        /// <param name="drug1">The drug1.</param>
        /// <param name="allergen1">The allergen1.</param>
        /// <param name="allergyMessage">The allergy message.</param>
        /// <param name="allergySeverity">The allergy severity.</param>
        /// <param name="allergyXrGroupDescription">The allergy xr group description.</param>
        /// <param name="allergyXrGroupId">The allergy xr group identifier.</param>
        /// <param name="classDescription">The class description.</param>
        /// <param name="classDescriptionPlural">The class description plural.</param>
        /// <param name="classId">The class identifier.</param>
        /// <param name="matchTypeDescription">The match type description.</param>
        /// <param name="matchTypeId">The match type identifier.</param>
        /// <param name="reaction">The reaction.</param>
        public DrugAllergyInterResult(Drug drug1, Allergen allergen1, string allergyMessage, string allergySeverity, string allergyXrGroupDescription, int allergyXrGroupId,
            string classDescription, string classDescriptionPlural, int classId, string matchTypeDescription, int matchTypeId, string reaction)
        {
            Drug1 = drug1;
            Allergen1 = allergen1;
            AllergyMessage = allergyMessage;
            AllergySeverity = allergySeverity;
            AllergyXrGroupDescription = allergyXrGroupDescription;
            AllergyXrGroupId = allergyXrGroupId;
            ClassDescription = classDescription;
            ClassDescriptionPlural = classDescriptionPlural;
            ClassId = classId;
            MatchTypeDescription = matchTypeDescription;
            MatchTypeId = matchTypeId;
            Reaction = reaction;
        }
        #endregion

        #region Properties
        [DataMember(IsRequired = false)]
        public Drug Drug1 { get; set; }

        [DataMember(IsRequired = false)]
        public Allergen Allergen1 { get; set; }

        [DataMember(IsRequired = false)]
        public string AllergyMessage { get; set; }

        [DataMember(IsRequired = false)]
        public string AllergySeverity { get; set; }

        [DataMember(IsRequired = false)]
        public string AllergyXrGroupDescription { get; set; }

        [DataMember(IsRequired = false)]
        public int AllergyXrGroupId { get; set; }

        [DataMember(IsRequired = false)]
        public string ClassDescription { get; set; }

        [DataMember(IsRequired = false)]
        public string ClassDescriptionPlural { get; set; }

        [DataMember(IsRequired = false)]
        public int ClassId { get; set; }

        [DataMember(IsRequired = false)]
        public string MatchTypeDescription { get; set; }

        [DataMember(IsRequired = false)]
        public int MatchTypeId { get; set; }

        [DataMember(IsRequired = false)]
        public string Reaction { get; set; }

        #endregion
    }
}