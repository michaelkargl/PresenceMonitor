using System.Net;
using Configuration;
using Exceptions;
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
        this._presenceService = CreatePresenceService(
            MockCreatePresenceApiOptions(),
            this._fakeHttpRequestHandler);
    }

    private static PresenceService CreatePresenceService(
        IMock<IOptions<PresenceApiOptions>> apiOptions,
        HttpMessageHandler httpRequestHandler
    ) => new (
        new HttpClient(httpRequestHandler, true),
        apiOptions.Object
    );

    private static Mock<IOptions<PresenceApiOptions>> MockCreatePresenceApiOptions()
    {
        var mock = new Mock<IOptions<PresenceApiOptions>>();
        mock
            .Setup(o => o.Value)
            .Returns(new PresenceApiOptions(url: "http://dummyurl.test"));

        return mock;
    }

    [Fact]
    public async Task GivenSuccessfulResponseItReturnsPresenceCount()
    {
        var expectedCount = Convert.ToByte(Random.Shared.Next(0, 128));
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

        exception?.Message.Should().NotBeNullOrWhiteSpace();
        exception!.Message.Should().Be(expectedErrorMessage);
    }
}