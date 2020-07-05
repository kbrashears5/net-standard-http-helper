﻿using Logger;

namespace HttpHelper.Test
{
    internal static class TestValues
    {
        internal static string Username { get; } = nameof(Username);

        internal static string Password { get; } = nameof(Password);

        internal static LogLevel LogLevel { get; } = LogLevel.Information;

        internal static string LogName { get; } = nameof(LogName);

        internal static ILogger Logger_Null { get; }

        internal static ILogger Logger_Mock { get; } = new Logger_Mock(logLevel: LogLevel,
            logName: LogName);

        internal static string ClientId { get; } = nameof(ClientId);

        internal static string ClientSecret { get; } = nameof(ClientSecret);

        internal static string AuthUrl { get; } = nameof(AuthUrl);

        internal static IHttpHelper HttpHelper { get; } = new HttpHelper(logger: Logger_Mock);

        internal static IHttpHelper HttpHelper_Mock_Good { get; } = new HttpHelper_Mock(returnSuccessStatusCode: true);

        internal static IHttpHelper HttpHelper_Mock_Bad { get; } = new HttpHelper_Mock(returnSuccessStatusCode: false);

        internal static string Body { get; } = nameof(Body);

        internal static string Url { get; } = "https://url.com";
    }
}