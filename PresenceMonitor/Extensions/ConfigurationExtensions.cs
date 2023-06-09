using Microsoft.Extensions.Configuration;

namespace Extensions;

internal static class ConfigurationExtensions
{
    public static TOption? GetOption<TOption>(this IConfiguration configuration) where TOption : class
        => configuration.GetSection(typeof(TOption).Name).Get<TOption>();

    public static TOption GetRequiredOption<TOption>(this IConfiguration configuration) where TOption : class
        => configuration.GetOption<TOption>()!;
}