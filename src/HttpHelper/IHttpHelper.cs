using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HttpHelper.Auth;

namespace HttpHelper
{
    /// <summary>
    /// Functions to interact with HTTP Clients
    /// </summary>
    public interface IHttpHelper
    {
        /// <summary>
        /// <see cref="HttpMethod.Delete"/> verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);

        /// <summary>
        /// <see cref="HttpMethod.Get"/> verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);

        /// <summary>
        /// Get OAuth2.0 Client Credentials token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="authUrl"></param>
        /// <returns></returns>
        Task<ClientCredentials> GetOAuth2ClientCredentialsAsync(string clientId,
            string clientSecret,
            string authUrl);

        /// <summary>
        /// HttpMethod.Patch verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PatchAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);

        /// <summary>
        /// <see cref="HttpMethod.Post"/> verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);

        /// <summary>
        /// <see cref="HttpMethod.Post"/> verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string url,
            FormUrlEncodedContent form,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);

        /// <summary>
        /// <see cref="HttpMethod.Put"/> verb
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false);
    }
}