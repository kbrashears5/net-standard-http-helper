using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
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

        #region IDisposable

        /// <summary>
        /// Disposed
        /// </summary>
        private bool Disposed { get; set; } = false;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }

                this.Disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~HttpHelper_Mock() => this.Dispose(disposing: false);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public async Task<HttpResponseMessage> DeleteAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> GetAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
        {
            return await this.ResponseMessage();
        }

        public async Task<ClientCredentials> GetOAuth2ClientCredentialsAsync(string clientId,
            string clientSecret,
            string authUrl,
            CancellationToken cancellationToken = default)
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
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> PostAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> PostAsync(string url,
            FormUrlEncodedContent form,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
        {
            return await this.ResponseMessage();
        }

        public async Task<HttpResponseMessage> PutAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false,
            CancellationToken cancellationToken = default)
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