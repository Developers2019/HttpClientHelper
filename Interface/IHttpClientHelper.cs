using HttpClientLibrary.Model;

namespace HttpClientLibrary.Interface
{
    public interface IHttpClientHelper<T>
    {
        Task<HttpResponseMessage> HttpDeleteAsync(string RequestUri);
        Task<HttpResponseMessage> HttpGetAccessTokenAsync(AccessTokenModel model);
        Task<List<T>> HttpGetAllAsync(string RequestUri);
        Task<List<U>> HttpGetAllAsync<U>(string RequestUri);

        Task<T> HttpGetByIdAsync(string RequestUri);
        Task<U> HttpGetByIdAsync<U>(string RequestUri);



        Task<HttpResponseMessage> HttpPostAsync(string RequestUri, T model);
        Task<HttpResponseMessage> HttpPostAsync<U>(string RequestUri, U model);
        Task<HttpResponseMessage> HttpPutAsync(string RequestUri, T model);
        Task<HttpResponseMessage> HttpPutAsync<U>(string RequestUri, U model);
    }
}
