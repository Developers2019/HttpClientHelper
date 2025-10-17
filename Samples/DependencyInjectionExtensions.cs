using HttpClientLibrary.HttpClientService;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace HttpClientLibrary.Samples
{
    /// <summary>
    /// Extension methods to register the HttpClientHelper with DI and Polly resilience policies.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Register the typed HttpClient helper and add default Polly retry and circuit-breaker policies.
        /// Usage example in a generic host / minimal API Program.cs:
        /// <code>
        /// var builder = WebApplication.CreateBuilder(args);
        /// builder.Services.AddHttpClientHelper(client => client.BaseAddress = new Uri("https://api.example.com"));
        /// var app = builder.Build();
        /// </code>
        /// </summary>
        public static IServiceCollection AddHttpClientHelper(
            this IServiceCollection services,
            Action<HttpClient>? configureClient = null,
            IEnumerable<TimeSpan>? retryDelays = null,
            int exceptionsAllowedBeforeBreaking = 5,
            TimeSpan? breakDuration = null)
        {
            var delays = retryDelays ??
            [
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5)
            ];

            var breakDur = breakDuration ?? TimeSpan.FromSeconds(30);

            services.AddHttpClient<IHttpClientHelper, HttpClientHelper>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(100);
                configureClient?.Invoke(client);
            })
            .AddPolicyHandler(GetRetryPolicy(delays))
            .AddPolicyHandler(GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking, breakDur));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IEnumerable<TimeSpan> delays)
        {
            // Retries on transient failures (5xx, 408) with configurable exponential backoff delays
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(delays);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, TimeSpan breakDuration)
        {
            // Break the circuit after a configurable number of consecutive failures
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, breakDuration);
        }
    }
}
