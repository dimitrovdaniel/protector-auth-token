using ProtectorTokenAuth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProtectorTokenAuth.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// The time in days for which the access token is valid.
        /// </summary>
        int TokenExpireTimeDays { get; set; }

        /// <summary>
        /// Creates the Access Token string based on the TokenPayload object.
        /// </summary>
        /// <param name="payload">The TokenPayload object containing user information.</param>
        /// <returns></returns>
        string CreateAccessToken(TokenPayload payload);
        /// <summary>
        /// Validates an Access Token string based on specified roles.
        /// </summary>
        /// <param name="accessToken">The Access Token string.</param>
        /// <param name="roles">Any roles you want to check if it's valid against.</param>
        /// <returns></returns>
        bool ValidateAccessToken(string accessToken, string[] roles);
        /// <summary>
        /// Creates a hash of the password string for storing into your database.
        /// </summary>
        /// <param name="password">The password string to hash.</param>
        /// <param name="salt">Optional salt for better hash security.</param>
        /// <returns></returns>
        string HashPassword(string password, string salt = null);
        /// <summary>
        /// The currently authenticated user.
        /// </summary>
        string ActiveUser { get; }
        /// <summary>
        /// The currently authenticated user roles.
        /// </summary>
        string[] ActiveUserRoles { get; }
    }
}
