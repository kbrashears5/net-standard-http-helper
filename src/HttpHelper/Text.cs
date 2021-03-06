﻿using System.Net;
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

        internal static string Url(string url) => $"URL: {url}";

        internal static string Request(object request) => $"Request: {JsonConvert.SerializeObject(value: request)}";

        internal static string Response(object response) => $"Response: {JsonConvert.SerializeObject(value: response)}";

        internal static string BadStatusCode(HttpStatusCode statusCode) => $"Endpoint returned bad status: {statusCode}";

        #endregion LOGGING

        #region VERBS

        internal static string Patch { get; } = nameof(Patch).ToUpper();

        #endregion VERBS
    }
}