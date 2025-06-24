using OrleansContracts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddKeyedAzureTableClient("clustering");
builder.UseOrleansClient();

builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//app.UseOutputCache(); // TODO make it work

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
        //.CacheOutput(); TODO make it work

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Orleans Client API V1");
    });
}

app.UseHttpsRedirection();


app.MapGet("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
    var grain = client.GetGrain<ICounterGrain>(grainId);
    return await grain.Get();
});

app.MapPost("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
    var grain = client.GetGrain<ICounterGrain>(grainId);
    return await grain.Increment();
});

app.UseFileServer();

await app.RunAsync();

