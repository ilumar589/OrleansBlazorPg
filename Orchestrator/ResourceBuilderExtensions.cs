using System.Diagnostics;

namespace Orchestrator;

internal static class ResourceBuilderExtensions
{

    internal static IResourceBuilder<T> WithSwaggerUI<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            swaggerCommand: "swagger-ui-docs",
            swaggerCommandDescription: "Swagger API Documentation",
            openApiUiPath: "swagger");
    }

    private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,
        string swaggerCommand,
        string swaggerCommandDescription,
        string openApiUiPath)
        where T : IResourceWithEndpoints
    {
        return builder
            .WithCommand(
                swaggerCommand,
                swaggerCommandDescription,
                executeCommand: async _ =>
                {
                    try
                    {
                        // Base URL
                        var endpoint = builder.GetEndpoint("https");

                        var url = $"{endpoint.Url}/{openApiUiPath}";

                        Process.Start(new ProcessStartInfo(url)
                        {
                            UseShellExecute = true,
                        });

                        return new ExecuteCommandResult
                        {
                            Success = true,
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ExecuteCommandResult
                        {
                            Success = false,
                            ErrorMessage = ex.Message,
                        };
                    }
                },
                updateState: context => context.ResourceSnapshot.HealthStatus == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy ?
                    ResourceCommandState.Enabled : ResourceCommandState.Disabled,
                iconName: "Document",
                iconVariant: IconVariant.Filled);
    }
}
