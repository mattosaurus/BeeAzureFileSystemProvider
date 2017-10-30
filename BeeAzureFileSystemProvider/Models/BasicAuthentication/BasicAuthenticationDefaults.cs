using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.BasicAuthentication
{
    /// <summary>
    /// The basic authentication defaults.
    /// </summary>
    public static class BasicAuthenticationDefaults
    {
        #region Constants

        /// <summary>
        /// The default value used for BasicAuthenticationOptions.AuthenticationScheme.
        /// </summary>
        public const string AuthenticationScheme = "Basic";

        /// <summary>
        /// The default value used for BasicAuthenticationOptions.Realm.
        /// </summary>
        public const string Realm = "Basic Realm";

        #endregion
    }
}
