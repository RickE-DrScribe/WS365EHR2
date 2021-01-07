using System.Runtime.Serialization;

namespace WS365EHR.Models
{
    /// <summary>
    /// Class SPParam.
    /// </summary>
    [DataContract]
    public class SPParam
    {
        #region Constructors / Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SPParam"/> class.
        /// </summary>
        public SPParam() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPParam"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public SPParam(string name, object value)
        {
            Name = name;
            Value = value;
        }
        #endregion

        #region Properties
        [DataMember]
        public string Name { get; set; }

        [DataMember(IsRequired=false)]
        public object Value { get; set; }

        #endregion
    }
}