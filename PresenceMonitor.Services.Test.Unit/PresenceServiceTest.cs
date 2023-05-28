using System.Net;
using Configuration;
using Exceptions;
using Extensions;
using Fakes;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

public class PresenceServiceTest
{
    private readonly PresenceService _presenceService;
    private readonly FakeHttpRequestHandler _fakeHttpRequestHandler;

    public PresenceServiceTest()
    {
        this._fakeHttpRequestHandler = new FakeHttpRequestHandler(HttpStatusCode.OK, string.Empty);
        this._presenceService = new PresenceService(
            CreateFakeHttpClient(this._fakeHttpRequestHandler),
            MockPresenceApiOptions().Object
        );
    }

    private static HttpClient CreateFakeHttpClient(HttpMessageHandler httpRequestHandler)
    {
        const string dummyUri = "http://127.0.0.1/dummy-test-base";
        
        var httpClient = new HttpClient(httpRequestHandler, true);
        httpClient.BaseAddress = new Uri(dummyUri);
        return httpClient;
    }

    private static Mock<IOptions<PresenceApiOptions>> MockPresenceApiOptions()
    {
        var mock = new Mock<IOptions<PresenceApiOptions>>();
        mock
            .Setup(o => o.Value)
            .Returns(new PresenceApiOptions("dummyAppId", "/dummyAppMethod"));

        return mock;
    }

    [Fact]
    public async Task GivenSuccessfulResponseItReturnsPresenceCount()
    {
        var expectedCount = Random.Shared.NextByte();
        this._fakeHttpRequestHandler.SetExpectedResponse(HttpStatusCode.OK, expectedCount.ToString());

        var result = await this._presenceService.GetPresenceCountAsync();

        result.Should().Be(expectedCount);
    }

    [Fact]
    public async Task GivenUnsuccessfulResponseItThrows()
    {
        var expectedErrorMessage = PresenceApiRequestFailedException.BuildMessage(
            nameof(this._presenceService.GetPresenceCountAsync),
            "Expected fake test failure");
        this._fakeHttpRequestHandler.SetExpectedResponse(HttpStatusCode.InternalServerError, expectedErrorMessage);

        var exception = await Assert.ThrowsAsync<PresenceApiRequestFailedException>(() =>
            this._presenceService.GetPresenceCountAsync());

        exception.Message.Should().NotBeNullOrWhiteSpace();
        exception.Message.Should().Be(expectedErrorMessage);
    }
}