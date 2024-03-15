namespace HttpClientLibrary.Helpers
{
    public static class Helper
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string HttpClientName = "HttpClientHelper";
        public static HttpResponseMessage ValidateResponse(
            this HttpResponseMessage response, 
            string url, 
            string message)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                // Handle unsuccessful request
                throw new HttpRequestException($"Failed to {message} from {url}. Status code: {response.StatusCode}");
            }
        }
    }
}
