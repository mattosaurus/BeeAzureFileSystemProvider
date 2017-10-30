using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.BasicAuthentication.Events
{
    /// <summary>
    /// The basic authentication events.
    /// </summary>
    public class BasicAuthenticationEvents
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a delegate assigned to this property will be invoked when the related method is called.
        /// </summary>
        public Func<ValidatePrincipalContext, Task<AuthenticateResult>> OnValidatePrincipal { get; set; } =
            context => Task.FromResult(AuthenticateResult.Fail("Incorrect credentials."));

        #endregion

        #region Implemented Interfaces

        #region IBasicAuthenticationEvents

        /// <summary>
        /// Called each time a request principal has been validated by the middleware. By implementing this method the
        /// application have alter or reject the principal which has arrived with the request.
        /// </summary>
        /// <param name="context">
        /// Contains information about the login session as well as the user name and provide password.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the completed operation.
        /// </returns>
        public virtual Task<AuthenticateResult> ValidatePrincipal(ValidatePrincipalContext context) => this.OnValidatePrincipal(context);

        #endregion

        #endregion
    }
}
