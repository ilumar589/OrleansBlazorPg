using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

// Add the resources which you will use for Orleans clustering and
// grain state storage.
var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var clusteringTable = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grain-state");

// Add the Orleans resource to the Aspire DistributedApplication
// builder, then configure it with Azure Table Storage for clustering
// and Azure Blob Storage for grain storage.
var orleans = builder.AddOrleans("default")
                     .WithClustering(clusteringTable)
                     .WithGrainStorage("Default", grainStorage);

builder.AddProject<Projects.OrleansServer>("orleansserver")
    .WithReference(orleans)
    .WithReplicas(3);

// config variables for client commands
var swaggerCommand = "swagger-ui-docs";
var swaggerCommandDescription = "Swagger UI for the Orleans server";
var openApiUiPath = "swagger";

var apiService = builder.AddProject<Projects.OrleansClient>("orleansclient");

apiService
    .WithReference(orleans.AsClient())
    .WithExternalHttpEndpoints()
    .WithReplicas(3)
    .WithCommand(
        swaggerCommand,
        swaggerCommandDescription,
        executeCommand: async _ =>
        {
            try
            {
                // Base URL
                var endpoint = apiService.GetEndpoint("https");

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

builder.AddProject<Projects.BzUI>("UI");

builder.Build().Run();
