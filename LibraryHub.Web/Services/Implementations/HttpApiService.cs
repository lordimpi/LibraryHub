using System.Net.Http.Json;
using LibraryHub.Web.Configurations;
using LibraryHub.Web.Services.Contracts;
using Microsoft.Extensions.Options;

namespace LibraryHub.Web.Services.Implementations;

/// <summary>
/// Implementa operaciones HTTP genéricas para consumo del backend desde MVC.
/// </summary>
public class HttpApiService : IHttpApiService
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsMonitor<ApiClientOptions> _apiClientOptions;

    /// <summary>
    /// Inicializa una nueva instancia del servicio HTTP de API.
    /// </summary>
    /// <param name="httpClient">Cliente HTTP inyectado por fábrica.</param>
    /// <param name="apiClientOptions">Opciones monitoreadas para la URL base.</param>
    public HttpApiService(HttpClient httpClient, IOptionsMonitor<ApiClientOptions> apiClientOptions)
    {
        _httpClient = httpClient;
        _apiClientOptions = apiClientOptions;
    }

    /// <inheritdoc />
    public async Task<TResponse?> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BuildUri(url), cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BuildUri(url), payload, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(BuildUri(url), payload, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsJsonAsync(BuildUri(url), payload, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> DeleteAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(BuildUri(url), cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    private string BuildUri(string relativeOrAbsoluteUrl)
    {
        if (Uri.TryCreate(relativeOrAbsoluteUrl, UriKind.Absolute, out _))
        {
            return relativeOrAbsoluteUrl;
        }

        var baseUrl = _apiClientOptions.CurrentValue.BaseUrl?.TrimEnd('/')
            ?? throw new InvalidOperationException("ApiClient:BaseUrl is not configured.");

        var relativeUrl = relativeOrAbsoluteUrl.TrimStart('/');
        return $"{baseUrl}/{relativeUrl}";
    }

    private static async Task<TResponse?> DeserializeResponse<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }
}
