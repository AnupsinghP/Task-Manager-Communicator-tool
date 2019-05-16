using System;
using Microsoft.AspNetCore.Http;

namespace CommunicationTool.Helper
{
    /// <summary>
    /// Authentication helper.
    /// </summary>
    public class AuthenticationHelper
    {
        /// <summary>
        /// Validated Seesion context
        /// </summary>
        /// <returns><c>true</c>, if authorized user <c>false</c> otherwise.</returns>
        /// <param name="context">Username.</param>
        public static bool isAuthorizedUser(string context) {
            var s = context;

            if (s == null)
            {
                return false;
            }
            return true;
        }
    }
}
