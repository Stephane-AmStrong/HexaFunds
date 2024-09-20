using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;

namespace WebApi.IntegrationTests.Setup;

public class HttpClientRepository
{
    private static HttpClient _httpClient = null!;
    public string RequestUri { get; set; }

    private readonly JsonSerializerOptions _options;

    public HttpClientRepository(HttpClient httpClient, string requestUri)
    {
        _httpClient = httpClient;
        RequestUri = requestUri;

        _httpClient.DefaultRequestHeaders.Clear();

        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<HttpResponseMessage> Get()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, RequestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<HttpResponseMessage> Get<TQuery>(TQuery query)
    {
        Dictionary<string, string> queryParams;

        if (query is not null)
        {
            queryParams = typeof(TQuery).GetProperties().Where(x => x.GetValue(query) is not null).Select(prop => new KeyValuePair<string, string>(prop.Name, prop.GetValue(query)?.ToString()!)).ToDictionary(prop => prop.Key, kvp => kvp.Value);
            var queryBuilder = new QueryBuilder(queryParams);

            RequestUri = Path.Combine(RequestUri, queryBuilder.ToString());
        }

        var request = new HttpRequestMessage(HttpMethod.Get, RequestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<HttpResponseMessage> GetById<TResponse>(Guid id) where TResponse : class
    {
        var uri = Path.Combine(RequestUri, id.ToString());
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<HttpResponseMessage> Create<TRequest>(TRequest tRequest) where TRequest : class
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, tRequest);
        ms.Seek(0, SeekOrigin.Begin);

        var request = new HttpRequestMessage(HttpMethod.Post, RequestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var requestContent = new StreamContent(ms);
        request.Content = requestContent;
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<HttpResponseMessage> Update<TRequest>(Guid id, TRequest tRequest) where TRequest : class
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, tRequest);
        ms.Seek(0, SeekOrigin.Begin);

        var uri = Path.Combine(RequestUri, id.ToString());
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var requestContent = new StreamContent(ms);
        request.Content = requestContent;
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<HttpResponseMessage> Delete(Guid id)
    {
        var uri = Path.Combine(RequestUri, id.ToString());
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    }



    public async Task<TResponse> DeserializeAsync<TResponse>(HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<TResponse>(stream, _options);

        return result == null
            ? throw new InvalidOperationException($"Failed to deserialize the response to type {typeof(TResponse)}.")
            : result;
    }
}
