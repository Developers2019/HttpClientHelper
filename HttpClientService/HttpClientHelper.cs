using HttpClientLibrary.Helpers;
using HttpClientLibrary.Interface;
using HttpClientLibrary.Model;
using System.Net.Http.Json;

namespace HttpClientLibrary.HttpClientService
{
	public class HttpClientHelper<T>(IHttpClientFactory httpClientFactory) : IHttpClientHelper<T>
	{
		public async Task<List<T>> HttpGetAllAsync(string RequestUri)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			var response = await _httpClient.GetAsync(RequestUri);

			var validatedResponse = response.ValidateResponse(RequestUri, Helper.GET);

			var result = await validatedResponse.Content.ReadFromJsonAsync<List<T>>();

			return result!;
		}

		public async Task<List<U>> HttpGetAllAsync<U>(string RequestUri)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			var response = await _httpClient.GetAsync(RequestUri);
			var validatedResponse = response.ValidateResponse(RequestUri, Helper.GET);

			var result = await validatedResponse.Content.ReadFromJsonAsync<List<U>>();

			return result!;
		}

		public async Task<T> HttpGetByIdAsync(string RequestUri)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			var response = await _httpClient.GetAsync(RequestUri);

			var validatedResponse = response.ValidateResponse(RequestUri, Helper.GET);

			var result = await validatedResponse.Content.ReadFromJsonAsync<T>();

			return result!;
		}

		public async Task<U> HttpGetByIdAsync<U>(string RequestUri)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			var response = await _httpClient.GetAsync(RequestUri);

			var validatedResponse = response.ValidateResponse(RequestUri, Helper.GET);

			var result = await validatedResponse.Content.ReadFromJsonAsync<U>();

			return result!;
		}

		public async Task<HttpResponseMessage> HttpPostAsync(string RequestUri, T model)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, model);
			return response.ValidateResponse(RequestUri, Helper.POST);
		}

		public async Task<HttpResponseMessage> HttpPostAsync<U>(string RequestUri, U model)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, model);
			return response.ValidateResponse(RequestUri, Helper.POST);
		}

		public async Task<HttpResponseMessage> HttpPutAsync(string RequestUri, T model)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			HttpResponseMessage response = await _httpClient.PutAsJsonAsync(RequestUri, model);
			return response.ValidateResponse(RequestUri, Helper.PUT);
		}

		public async Task<HttpResponseMessage> HttpPutAsync<U>(string RequestUri, U model)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			HttpResponseMessage response = await _httpClient.PutAsJsonAsync(RequestUri, model);
			return response.ValidateResponse(RequestUri, Helper.PUT);
		}

		public async Task<HttpResponseMessage> HttpDeleteAsync(string RequestUri)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			HttpResponseMessage response = await _httpClient.DeleteAsync(RequestUri);
			return response.ValidateResponse(RequestUri, Helper.DELETE);
		}

		public async Task<HttpResponseMessage> HttpGetAccessTokenAsync(AccessTokenModel model)
		{
			var _httpClient = httpClientFactory.CreateClient(Helper.HttpClientName);

			List<KeyValuePair<string, string>> keyValues =
				[
					new KeyValuePair<string, string>("username",model.Username),
					new KeyValuePair<string, string>("password",model.Password),
					new KeyValuePair<string, string>("grant_type",model.GrantType)
				];

			HttpRequestMessage request = new(HttpMethod.Post, model.RequestUrl)
			{
				Content = new FormUrlEncodedContent(keyValues)
			};

			HttpResponseMessage response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			return response;
		}
	}
}