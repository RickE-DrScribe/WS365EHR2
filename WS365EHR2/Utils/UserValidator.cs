using System;
using System.ServiceModel;
using System.IdentityModel.Selectors;

namespace WS365EHR
{
    /// <summary>
    /// Class UserValidator.
    /// Implements the <see cref="System.IdentityModel.Selectors.UserNamePasswordValidator" />
    /// </summary>
    /// <remarks>DO NOT REMOVE FROM PROJECT. Even though it is not used in the code it is used
    /// by the web service for validation. </remarks>
    public class UserValidator : UserNamePasswordValidator
    {
        /// <summary>
        /// When overridden in a derived class, validates the specified username and password.
        /// </summary>
        /// <param name="userName">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FaultException">Password or name is wrong</exception>
        public override void Validate(string userName, string password)
        {
            if (userName == null || password == null)
            {
                throw new ArgumentNullException();
            }

            if (!(userName == "DrSW@6BS4L!f3" && password == "G1@d3c00L1975"))
            {
                throw new FaultException("Password or name is wrong");
            }
        }
    }
}