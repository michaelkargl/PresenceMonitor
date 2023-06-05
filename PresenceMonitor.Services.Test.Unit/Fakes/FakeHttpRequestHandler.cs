using System.Net;

namespace Fakes;

internal class FakeHttpRequestHandler : DelegatingHandler
{
    private HttpResponseMessage _expectedResponse = null!;

    public FakeHttpRequestHandler(HttpStatusCode expectedStatus, string expectedContent)
    {
        this.SetExpectedResponse(expectedStatus, expectedContent);
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    ) => Task.FromResult(this._expectedResponse);

    internal void SetExpectedResponse(HttpStatusCode expectedStatus, string expectedContent)
        => this._expectedResponse = new HttpResponseMessage()
        {
            StatusCode = expectedStatus,
            Content = new StringContent(expectedContent)
        };
}