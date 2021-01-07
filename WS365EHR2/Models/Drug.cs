using System.Runtime.Serialization;

namespace WS365EHR.Models
{
    /// <summary>
    /// Class Drug.
    /// </summary>
    [DataContract]
    public class Drug
    {
        #region Constructors / Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class.
        /// </summary>
        public Drug() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class.
        /// </summary>
        /// <param name="genDrugId">The gen drug identifier.</param>
        /// <param name="genericName">Name of the generic.</param>
        public Drug(string genDrugId, string genericName)
        {
            GenDrugId = genDrugId;
            GenericName = genericName;
        }
        #endregion

        #region Properties
        [DataMember(IsRequired = false)]
        public string GenDrugId { get; set; }

        [DataMember(IsRequired = false)]
        public string GenericName { get; set; }

        #endregion
    }
}