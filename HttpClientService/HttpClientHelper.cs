using HttpClientLibrary.Model;
using System.Net.Http.Json;

namespace HttpClientLibrary.HttpClientService
{
    public class HttpClientHelper(HttpClient _httpClient) : IHttpClientHelper
    {
        /// <summary>
        /// Retrieves a list of items from the specified URI asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of items to retrieve.</typeparam>
        /// <param name="RequestUri">The URI of the resource to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of items of type <typeparamref name="T"/>.</returns>
        public async Task<List<T>> HttpRetrieveAllAsync<T>(string RequestUri)
        {
            var httpResponse = await _httpClient.GetAsync(RequestUri);
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<List<T>>();

            return result!;
        }

        /// <summary>
        /// Retrieves an item by ID from the specified URI asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="RequestUri">The URI of the resource to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an item of type <typeparamref name="T"/>.</returns>
        public async Task<T> HttpRetrieveByIdAsync<T>(string RequestUri)
        {
            var httpResponse = await _httpClient.GetAsync(RequestUri);
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<T>();

            return result!;
        }

        /// <summary>
        /// Sends a POST request with a model to the specified URI asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the model to send.</typeparam>
        /// <param name="RequestUri">The URI of the resource to send the request to.</param>
        /// <param name="model">The model to send in the request body.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        public async Task<HttpResponseMessage> HttpPostAsync<T>(string RequestUri, T model)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(RequestUri, model);
            httpResponse.EnsureSuccessStatusCode();

            return httpResponse;
        }

        /// <summary>
        /// Sends a PUT request with a model to the specified URI asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the model to send.</typeparam>
        /// <param name="RequestUri">The URI of the resource to send the request to.</param>
        /// <param name="model">The model to send in the request body.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        public async Task<HttpResponseMessage> HttpPutAsync<T>(string RequestUri, T model)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(RequestUri, model);
            httpResponse.EnsureSuccessStatusCode();

            return httpResponse;
        }

        /// <summary>
        /// Sends a DELETE request to the specified URI asynchronously.
        /// </summary>
        /// <param name="RequestUri">The URI of the resource to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        public async Task<HttpResponseMessage> HttpDeleteAsync(string RequestUri)
        {
            var httpResponse = await _httpClient.DeleteAsync(RequestUri);
            httpResponse.EnsureSuccessStatusCode();

            return httpResponse;
        }

        /// <summary>
        /// Retrieves an access token using the specified access token model asynchronously.
        /// </summary>
        /// <param name="model">The access token model containing the necessary parameters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        public async Task<HttpResponseMessage> HttpRetrieveAccessTokenAsync(AccessTokenModel model)
        {
            List<KeyValuePair<string, string>> keyValues = 
            [
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password),
                new KeyValuePair<string, string>("grant_type", model.GrantType)
            ];

            HttpRequestMessage request = new(HttpMethod.Post, model.RequestUrl)
            {
                Content = new FormUrlEncodedContent(keyValues)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

    }
}