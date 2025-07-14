using Orleans;
using OrleansContracts.ShoppingCart.Product;
using System.Text.Json.Serialization;

namespace OrleansContracts.ShoppingCart.Cart;

[GenerateSerializer, Immutable]
[Alias("OrleansContracts.ShoppingCart.Cart.CartItem")]
public sealed record CartItem
{
    [Id(0)] public required string UserId { get; init; }
    [Id(1)] public int Quantity { get; init; } = 0;
    [Id(2)] public required ProductDetails Product {  get; init; }

    [JsonIgnore]
    public decimal TotalPrice => Math.Round(Quantity * Product.UnitPrice, 2);
}
