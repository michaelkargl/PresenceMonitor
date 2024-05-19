using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules;
using Moq;
using Xunit.Abstractions;

namespace PresenceMonitor.Test;

// TODO: move to test infrastructure project
public class ContainerFixture : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public ContainerFixture()
    {
        var serviceCollection = new ServiceCollection()
            .AddSingleton(SetupConfiguration());

        RootModule.Configure(
            serviceCollection,
            SetupHostEnvironment(),
            SetupConfiguration());

        this._serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public System.IServiceProvider ServiceProvider => this._serviceProvider;

    public void Dispose()
    {
        this._serviceProvider.Dispose();
    }

    private static IHostEnvironment SetupHostEnvironment() => MockHostEnvironmentName(
        new Mock<IHostEnvironment>()
    ).Object;

    private static Mock<IHostEnvironment> MockHostEnvironmentName(Mock<IHostEnvironment> hostEnvironmentMock)
    {
        hostEnvironmentMock
            .Setup(environment => environment.EnvironmentName)
            .Returns("offline");
        return hostEnvironmentMock;
    }

    private static IConfiguration SetupConfiguration() => new ConfigurationBuilder()
        .Add(new JsonConfigurationSource
        {
            Path = "appsettings.test.json",
            Optional = false,
            ReloadOnChange = false
        }).Build();
}