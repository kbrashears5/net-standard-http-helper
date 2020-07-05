using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpHelper.Auth;

namespace HttpHelper
{
    /// <summary>
    /// Mock implementation of <see cref="IHttpHelper"/>
    /// </summary>
    public class HttpHelper_Mock : IHttpHelper
    {
        private bool ReturnSuccessStatusCode { get; }

        /// <summary>
        /// Create new instance of <see cref="HttpHelper_Mock"/>
        /// </summary>
        /// <param name="returnSuccessStatusCode"></param>
        public HttpHelper_Mock(bool returnSuccessStatusCode)
        {
            this.ReturnSuccessStatusCode = returnSuccessStatusCode;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public async Task<HttpResponseMessage> DeleteAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> GetAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.ResponseMessage();
        }

        public async Task<ClientCredentials> GetOAuth2ClientCredentialsAsync(string clientId,
            string clientSecret,
            string authUrl)
        {
            var clientCredentials = new ClientCredentials()
            {
                Access_Token = Text.AccessToken,
                Expires_In = 7199,
                Token_Type = Text.BearerToken,
            };

            return await Task.FromResult(clientCredentials);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> PostAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> PutAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.ResponseMessage();
        }

        private Task<HttpResponseMessage> ResponseMessage()
        {
            var responseCode = this.ReturnSuccessStatusCode ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return Task.FromResult(new HttpResponseMessage(statusCode: responseCode));
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}