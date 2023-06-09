using System.Net.Http.Json;
using System.Web;
using Configuration;
using Microsoft.Extensions.Options;
using static System.String;

public class DaprMessagePublisher : IMessagePublisher
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<DaprPublisherOptions> _options;

    public DaprMessagePublisher(HttpClient httpClient, IOptions<DaprPublisherOptions> options)
    {
        this._httpClient = httpClient;
        this._options = options;
    }

    private DaprPublisherOptions DaprOptions => this._options.Value;

    public async Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        var response = await this.PublishDaprMessage(
            message,
            this.DaprOptions.DaprEndpoint!,
            this.DaprOptions.PubSubName,
            this.DaprOptions.Topic,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private Task<HttpResponseMessage> PublishDaprMessage(
        object message,
        string daprEndpoint,
        string pubSubName,
        string pubSubTopic,
        CancellationToken cancellationToken
    )
    {
        var queries = HttpUtility.ParseQueryString(Empty);
        queries.Add("metadata.rawPayload", true.ToString());

        var uriBuilder = new UriBuilder(daprEndpoint)
        {
            Path = $"/v1.0/publish/{pubSubName}/{pubSubTopic}",
            Query = queries.ToString()
        };

        return this._httpClient.PostAsJsonAsync(
            uriBuilder.Uri,
            message,
            cancellationToken);
    }
}