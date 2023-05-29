using System.Net.Http.Json;
using Configuration;
using Exceptions;
using Microsoft.Extensions.Options;

public class RandomPresenceService : IPresenceService
{
    public Task<byte> GetPresenceCountAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[1];
        Random.Shared.NextBytes(buffer);
        return Task.FromResult(buffer.Single());
    }
} 

public class PresenceService : IPresenceService
{
    private readonly HttpClient _client;
    private readonly IOptions<PresenceApiOptions> _apiOptions;

    private PresenceApiOptions ApiOptions => this._apiOptions.Value;

    public PresenceService(HttpClient client, IOptions<PresenceApiOptions> apiOptions)
    {
        this._client = client;
        this._apiOptions = apiOptions;
    }
    
    public async Task<byte> GetPresenceCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await this._client.GetAsync(this.ApiOptions.Url, cancellationToken);
            var presenceCount = await response.Content.ReadFromJsonAsync<byte>(cancellationToken: cancellationToken);
            return presenceCount;
        }
        catch (Exception ex)
        {
            throw new PresenceApiRequestFailedException(nameof(this.GetPresenceCountAsync), ex.Message, ex);
        }
    }
}