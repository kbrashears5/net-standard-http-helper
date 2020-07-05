using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace HttpHelper
{
    internal static class Text
    {
        #region AUTH

        internal static string BasicAuth { get; } = "Basic";

        internal static string BearerToken { get; } = "Bearer";

        internal static string GrantType { get; } = "grant_type";

        internal static string ClientCredentials { get; } = "client_credentials";

        internal static string ClientId { get; } = "client_id";

        internal static string ClientSecret { get; } = "client_secret";

        internal static string AccessToken { get; } = "access_token";

        #endregion AUTH

        #region HEADERS

        internal const string ApplicationJson = "application/json";

        internal const string FormUrlEncoded = "application/x-www-form-urlencoded";

        internal const string None = "";

        #endregion HEADERS

        #region LOGGING

        internal static string Url(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            return $"URL: {url}";
        }

        internal static string Request(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return $"Request: {JsonConvert.SerializeObject(value: request)}";
        }

        internal static string Response(object response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            return $"Response: {JsonConvert.SerializeObject(value: response)}";
        }

        internal static string BadStatusCode(HttpStatusCode statusCode)
        {
            return $"Endpoint returned bad status: {statusCode}";
        }

        #endregion LOGGING

        #region VERBS

        internal static string Patch { get; } = nameof(Patch).ToUpper();

        #endregion VERBS
    }
}