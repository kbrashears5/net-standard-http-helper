namespace HttpHelper.Auth
{
    /// <summary>
    /// Represents a Client Credentials response
    /// </summary>
    public class ClientCredentials
    {
        /// <summary>
        /// Access Token
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// Token Type
        /// </summary>
        public string Token_Type { get; set; }

        /// <summary>
        /// Time to expiration, in seconds
        /// </summary>
        public int Expires_In { get; set; }
    }
}