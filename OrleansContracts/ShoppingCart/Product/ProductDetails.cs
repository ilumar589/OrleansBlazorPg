using Orleans;
using System.Text.Json.Serialization;

namespace OrleansContracts.ShoppingCart.Product;

[GenerateSerializer, Immutable]
[Alias("OrleansContracts.ShoppingCart.Product.ProductDetails")]
public sealed record ProductDetails
{
    [Id(0)] public string Id { get; init; } = Random.Shared.Next(1, 1_000_000).ToString();
    [Id(1)] public required string Name { get; init; }
    [Id(2)] public string Description { get; init; } = string.Empty;
    [Id(3)] public required ProductCategory Category { get; init; }
    [Id(4)] public int Quantity { get; init; } = 0;
    [Id(5)] public required decimal UnitPrice { get; init; }
    [Id(6)] public string? DetailsUrl { get; init; } = null;
    [Id(7)] public string? ImageUrl { get; init; } = null;

    [JsonIgnore]
    public decimal TotalPrice => Math.Round(UnitPrice * Quantity, 2);
}
