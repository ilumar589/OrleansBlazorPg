using Orleans;

namespace OrleansContracts.ShoppingCart.Product;

[Alias("OrleansContracts.ShoppingCart.Product.IInventoryGrain")]
public interface IInventoryGrain : IGrainWithStringKey
{
    [Alias("GetAllProductsAsync")]
    ValueTask<HashSet<ProductDetails>> GetAllProductsAsync();
    [Alias("AddOrUpdateProductAsync")]
    ValueTask AddOrUpdateProductAsync(ProductDetails productDetails);
    [Alias("RemoveProductAsync")]
    ValueTask RemoveProductAsync(string productId);
}
