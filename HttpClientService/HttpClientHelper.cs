using HttpClientLibrary.Model;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HttpClientLibrary.HttpClientService;

public class HttpClientHelper(
    HttpClient httpClient,
    ILogger<HttpClientHelper>? logger = null,
    JsonSerializerOptions? jsonOptions = null) : IHttpClientHelper
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger? _logger = logger;
    private readonly JsonSerializerOptions _jsonOptions = jsonOptions ?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<List<T>> HttpRetrieveAllAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        return await SendRequestAsync<List<T>>(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> HttpRetrieveByIdAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        return await SendRequestAsync<T>(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> HttpPostAsync<T>(string requestUri, T model, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, model, _jsonOptions, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
        finally
        {
            sw.Stop();
            _logger?.LogInformation("POST {Uri} took {Elapsed}ms", requestUri, sw.ElapsedMilliseconds);
        }
    }

    public async Task<HttpResponseMessage> HttpPutAsync<T>(string requestUri, T model, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, model, _jsonOptions, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
        finally
        {
            sw.Stop();
            _logger?.LogInformation("PUT {Uri} took {Elapsed}ms", requestUri, sw.ElapsedMilliseconds);
        }
    }

    public async Task<HttpResponseMessage> HttpDeleteAsync(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
        finally
        {
            sw.Stop();
            _logger?.LogInformation("DELETE {Uri} took {Elapsed}ms", requestUri, sw.ElapsedMilliseconds);
        }
    }

    public async Task<HttpResponseMessage> HttpRetrieveAccessTokenAsync(
        AccessTokenModel model,
        CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> formData = new()
        {
            { "username", model.Username },
            { "password", model.Password },
            { "grant_type", model.GrantType }
        };
        return await HttpPostFormAsync(model.RequestUrl, formData, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> SendRequestAsync<T>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            _logger?.LogInformation("{Method} {Uri} returned {StatusCode}", request.Method, request.RequestUri, response.StatusCode);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false);
            if (result == null)
                throw new InvalidOperationException("Deserialized response was null.");

            return result;
        }
        finally
        {
            sw.Stop();
            _logger?.LogInformation("{Method} {Uri} took {Elapsed}ms", request.Method, request.RequestUri, sw.ElapsedMilliseconds);
        }
    }

    private async Task<HttpResponseMessage> HttpPostFormAsync(
        string requestUri,
        Dictionary<string, string> formData,
        CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            using var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
        finally
        {
            sw.Stop();
            _logger?.LogInformation("POST-FORM {Uri} took {Elapsed}ms", requestUri, sw.ElapsedMilliseconds);
        }
    }
}