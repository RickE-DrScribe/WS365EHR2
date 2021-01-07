using System.Runtime.Serialization;

namespace WS365EHR.Models
{
    /// <summary>
    /// Class Allergen.
    /// </summary>
    [DataContract]
    public class Allergen
    {
        #region Constructors / Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Allergen"/> class.
        /// </summary>
        public Allergen() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Allergen"/> class.
        /// </summary>
        /// <param name="allergySeverity">The allergy severity.</param>
        /// <param name="conceptType">Type of the concept.</param>
        /// <param name="reaction">The reaction.</param>
        public Allergen(string allergySeverity, string conceptType, string reaction)
        {
            AllergySeverity = allergySeverity;
            ConceptType = conceptType;
            Reaction = reaction;
        }
        #endregion

        #region Properties
        [DataMember(IsRequired = false)]
        public string AllergySeverity { get; set; }

        [DataMember(IsRequired = false)]
        public string ConceptType { get; set; }

        [DataMember(IsRequired = false)]
        public string Reaction { get; set; }

        #endregion
    }
}