using HttpClientLibrary.Model;
using System.Threading;

namespace HttpClientLibrary.HttpClientService;

public interface IHttpClientHelper
{
    /// <summary>
    /// Sends a DELETE request to the specified URI asynchronously.
    /// </summary>
    /// <param name="RequestUri">The URI of the resource to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
    Task<HttpResponseMessage> HttpDeleteAsync(string requestUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an access token using the specified access token model asynchronously.
    /// </summary>
    /// <param name="model">The access token model containing the necessary parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
    Task<HttpResponseMessage> HttpRetrieveAccessTokenAsync(AccessTokenModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of items from the specified URI asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="RequestUri">The URI of the resource to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of items of type <typeparamref name="T"/>.</returns>
    Task<List<T>> HttpRetrieveAllAsync<T>(string requestUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an item by ID from the specified URI asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of item to retrieve.</typeparam>
    /// <param name="RequestUri">The URI of the resource to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an item of type <typeparamref name="T"/>.</returns>
    Task<T> HttpRetrieveByIdAsync<T>(string requestUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a POST request with a model to the specified URI asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the model to send.</typeparam>
    /// <param name="RequestUri">The URI of the resource to send the request to.</param>
    /// <param name="model">The model to send in the request body.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
    Task<HttpResponseMessage> HttpPostAsync<T>(string requestUri, T model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a PUT request with a model to the specified URI asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the model to send.</typeparam>
    /// <param name="RequestUri">The URI of the resource to send the request to.</param>
    /// <param name="model">The model to send in the request body.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
    Task<HttpResponseMessage> HttpPutAsync<T>(string requestUri, T model, CancellationToken cancellationToken = default);
}