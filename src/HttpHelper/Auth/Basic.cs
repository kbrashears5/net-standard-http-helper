using System;
using System.Text;

namespace HttpHelper.Auth
{
    /// <summary>
    /// Representation of Basic Credentials
    /// </summary>
    public class Basic
    {
        /// <summary>
        /// Base 64 Credentials
        /// </summary>
        public string Base64Credentials { get; }

        /// <summary>
        /// Create new instance of <see cref="Basic"/> credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Basic(string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var bytes = Encoding.UTF8.GetBytes(s: $"{username}:{password}");

            this.Base64Credentials = Convert.ToBase64String(inArray: bytes);
        }
    }
}