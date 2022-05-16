using System;
using System.Collections.Generic;
using System.Text;

namespace ProtectorTokenAuth.Models
{
    /// <summary>
    /// An object stored in the Access Token, keeping relative user information.
    /// </summary>
    public class TokenPayload
    {
        /// <summary>
        /// The time after which the token will no longer be valid.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        /// <summary>
        /// The username of the current user.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The roles of the current user.
        /// </summary>
        public string[] UserRoles { get; set; }
        /// <summary>
        /// Custom data for the current user.
        /// </summary>
        public Dictionary<string,string> CustomData { get; set; }
    }
}
