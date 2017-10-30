using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.BasicAuthentication
{
    /// <summary>
    /// The basic authentication extensions.
    /// </summary>
    public static class BasicAuthenticationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddBasicAuthentication(BasicAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        /// <param name="authenticationScheme">
        /// The authentication scheme.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return builder.AddBasicAuthentication(authenticationScheme, null);
        }

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        /// <param name="configureOptions">
        /// The configure options.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
        {
            return builder.AddBasicAuthentication(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="authenticationScheme">
        /// The authentication scheme.
        /// </param>
        /// <param name="configureOptions">
        /// The configure options.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<BasicAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, configureOptions);
        }

        #endregion
    }
}
