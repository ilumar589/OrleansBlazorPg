using Orchestrator;

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


var apiService = builder.AddProject<Projects.OrleansClient>("orleansclient");

apiService
    .WithReference(orleans.AsClient())
    .WithExternalHttpEndpoints()
    .WithReplicas(3)
    .WithSwaggerUI();

builder.AddProject<Projects.BzUI>("UI");

builder.Build().Run();
