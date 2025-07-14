using Orleans;

namespace OrleansContracts.ShoppingCart.Product;

[Alias("OrleansContracts.ShoppingCart.Product.IProductGrain")]
public interface IProductGrain : IGrainWithStringKey
{
    [Alias("TryTakeProductAsync")]
    ValueTask<(bool IsAvailable, ProductDetails ProductDetails)> TryTakeProductAsync(int quantity);
    [Alias("ReturnProductAsync")]
    ValueTask ReturnProductAsync(int quantity);
    [Alias("GetProductAvailibilityAsync")]
    ValueTask<int> GetProductAvailibilityAsync();
    [Alias("CreateOrUpdateProductAsync")]
    ValueTask CreateOrUpdateProductAsync(ProductDetails productDetails);
    [Alias("GetProductDetailsAsync")]
    ValueTask<ProductDetails> GetProductDetailsAsync();
}
