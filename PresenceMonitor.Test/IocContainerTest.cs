using Configuration;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PresenceMonitor.Utilities.Ioc;
using Xunit.Abstractions;

namespace PresenceMonitor.Test;

public class IocContainerTest : IClassFixture<ContainerFixture>
{
    private readonly ContainerFixture _containerFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IocContainerTest(ContainerFixture containerFixture, ITestOutputHelper testOutputHelper)
    {
        this._containerFixture = containerFixture;
        this._testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(typeof(IConfiguration))]
    [InlineData(typeof(IMessagePublisher<byte, RawDaprPublisherOptions>))]
    [InlineData(typeof(IMessagePublisher<PresenceMessageV1, PresenceMessageV1DaprPublisherOptions>))]
    [InlineData(typeof(ISingletonProvider<DaprClient>))]
    [InlineData(typeof(IDaprClientProvider))]
    public void TestServicesResolvable(Type service)
    {
        _testOutputHelper.WriteLine("Resolving type: {0}", service.FullName);
        var resolvedService = this.ServiceProvider.GetRequiredService(service);
        Assert.NotNull(resolvedService);
    }

    [Fact]
    public void SingletonProvider_DisposesScope()
    {
        var singletonProvider = this.ServiceProvider.GetRequiredService<ISingletonProvider<DaprClient>>();
        _ = singletonProvider.Provide();

        Assert.True(singletonProvider.ScopeActive);
        
        singletonProvider.Dispose();
        
        Assert.False(singletonProvider.ScopeActive);
    }

    private IServiceProvider ServiceProvider => this._containerFixture.ServiceProvider;
}