using System.Net.Http.Json;
using Configuration;
using Exceptions;
using Microsoft.Extensions.Options;

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
            return await this._client.GetFromJsonAsync<byte>(
                this.ApiOptions.GetPresenceCountMethod,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            throw new PresenceApiRequestFailedException(nameof(this.GetPresenceCountAsync), ex.Message, ex);
        }
    }
}