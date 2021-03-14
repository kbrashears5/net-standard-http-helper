using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Logger;
using HttpHelper.Auth;
using Newtonsoft.Json;

namespace HttpHelper
{
    /// <summary>
    /// Implementation of <see cref="IHttpHelper"/>
    /// </summary>
    public class HttpHelper : IHttpHelper
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// HTTP Client
        /// </summary>
        private HttpClient HttpClient { get; set; }

        /// <summary>
        /// Timer for auth tokens
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// Create new instance of <see cref="HttpHelper"/>
        /// </summary>
        /// <param name="logger"></param>
        public HttpHelper(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.InitializeClient();
        }

        /// <summary>
        /// Create new instance of <see cref="HttpHelper"/> using Basic Auth
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public HttpHelper(ILogger logger,
            string username,
            string password)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            this.InitializeClient();

            // create base 64 credentials
            var credentials = new Basic(username: username,
                password: password);

            // add basic auth to headers
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: Text.BasicAuth,
                parameter: credentials.Base64Credentials);
        }

        /// <summary>
        /// Create new instance of <see cref="HttpHelper"/> using OAuth 2.0 client_credential flow
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="authUrl"></param>
        public HttpHelper(ILogger logger,
            string clientId,
            string clientSecret,
            string authUrl)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(authUrl)) throw new ArgumentNullException(nameof(authUrl));

            this.InitializeClient();

            this.InitializeOAuth2ClientCredentials(sender: null,
                eventArgs: null,
                clientId: clientId,
                clientSecret: clientSecret,
                authUrl: authUrl).Wait();
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
                    this.Timer?.Dispose();

                    this.HttpClient?.Dispose();

                    this.Logger?.Dispose();
                }

                this.Disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~HttpHelper() => this.Dispose(disposing: false);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        /// <summary>
        /// Initialize the HTTP Client
        /// </summary>
        private void InitializeClient()
        {
            this.HttpClient = new HttpClient();
        }

        /// <summary>
        /// Initialize the OAuth2.0 Bearer Token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="authUrl"></param>
        /// <returns></returns>
        private async Task InitializeOAuth2ClientCredentials(object sender,
            EventArgs eventArgs,
            string clientId,
            string clientSecret,
            string authUrl)
        {
            // get auth token
            var clientCredentials = await this.GetOAuth2ClientCredentialsAsync(clientId: clientId,
                clientSecret: clientSecret,
                authUrl: authUrl);

            // add bearer token to headers
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: Text.BearerToken,
                parameter: clientCredentials.Access_Token);

            // start timer to get new token 30 seconds before expiration
            this.Timer = new Timer(interval: (clientCredentials.Expires_In - 30) * 1000);

            this.Timer.Elapsed += async (s, e) => await this.InitializeOAuth2ClientCredentials(sender: sender,
                eventArgs: eventArgs,
                clientId: clientId,
                clientSecret: clientSecret,
                authUrl: authUrl);

            this.Timer.Start();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public async Task<HttpResponseMessage> DeleteAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                httpMethod: HttpMethod.Delete,
                body: string.Empty,
                contentType: contentType,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        public async Task<HttpResponseMessage> GetAsync(string url,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                httpMethod: HttpMethod.Get,
                body: string.Empty,
                contentType: contentType,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        public async Task<ClientCredentials> GetOAuth2ClientCredentialsAsync(string clientId,
            string clientSecret,
            string authUrl)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(authUrl)) throw new ArgumentNullException(nameof(authUrl));

            var jsonBody = new Dictionary<string, string>()
            {
                { Text.GrantType, Text.ClientCredentials },
                { Text.ClientId, clientId},
                { Text.ClientSecret, clientSecret},
            };

            var form = new FormUrlEncodedContent(nameValueCollection: jsonBody);

            var response = await this.PostAsync(url: authUrl,
                form: form,
                throwOnBadStatus: true);

            var clientCredentials = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ClientCredentials>(clientCredentials);
        }

        /// <summary>
        /// Invoke <see cref="HttpMethod"/> async for string or json body
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> InvokeAsync(string url,
            HttpMethod httpMethod,
            string body,
            ContentType contentType,
            Dictionary<string, string> headers,
            bool throwOnBadStatus)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));
            if (httpMethod != HttpMethod.Get && string.IsNullOrWhiteSpace(body)) throw new ArgumentNullException(nameof(body));

            var content = new StringContent(content: body,
                encoding: Encoding.UTF8,
                mediaType: contentType.ToAttributeString());

            return await this.SendAsync(httpMethod: httpMethod,
                url: url,
                content: content,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        /// <summary>
        /// Invoke <see cref="HttpMethod"/> async for form body
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> InvokeAsync(string url,
            HttpMethod httpMethod,
            FormUrlEncodedContent content,
            Dictionary<string, string> headers,
            bool throwOnBadStatus)
        {
#pragma warning disable IDE0046 // Convert to conditional expression

            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await this.SendAsync(httpMethod: httpMethod,
                url: url,
                content: content,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);

#pragma warning restore IDE0046 // Convert to conditional expression
        }

        public async Task<HttpResponseMessage> PatchAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                 httpMethod: new HttpMethod(method: Text.Patch),
                 body: body,
                 contentType: contentType,
                 headers: headers,
                 throwOnBadStatus: throwOnBadStatus);
        }

        public async Task<HttpResponseMessage> PostAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                httpMethod: HttpMethod.Post,
                body: body,
                contentType: contentType,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        public async Task<HttpResponseMessage> PostAsync(string url,
            FormUrlEncodedContent form,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                httpMethod: HttpMethod.Post,
                content: form,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        public async Task<HttpResponseMessage> PutAsync(string url,
            string body,
            ContentType contentType = ContentType.None,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            return await this.InvokeAsync(url: url,
                httpMethod: HttpMethod.Put,
                body: body,
                contentType: contentType,
                headers: headers,
                throwOnBadStatus: throwOnBadStatus);
        }

        /// <summary>
        /// Send the Http command
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <param name="throwOnBadStatus"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod,
            string url,
            HttpContent content,
            Dictionary<string, string> headers = null,
            bool throwOnBadStatus = false)
        {
            if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (content == null) throw new ArgumentNullException(nameof(content));

            this.Logger.LogTrace(message: Text.Url(url));

            var uri = new Uri(uriString: url);

            var request = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = uri,
                Content = content,
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(name: header.Key,
                        value: header.Value);
                }
            }

            this.Logger.LogTrace(message: Text.Request(request));

            var response = await this.HttpClient.SendAsync(request: request);
            this.Logger.LogTrace(message: Text.Response(response));

            return throwOnBadStatus && !response.IsSuccessStatusCode
                ? throw new HttpRequestException(Text.BadStatusCode(response.StatusCode))
                : response;
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}