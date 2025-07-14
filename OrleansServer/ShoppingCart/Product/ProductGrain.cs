using OrleansContracts.ShoppingCart.Product;

namespace OrleansServer.ShoppingCart.Product;

public sealed class ProductGrain(
    [PersistentState(
        stateName: "Product",
        storageName: "shopping-cart")]
    IPersistentState<ProductDetails> product) : IProductGrain
{

    private readonly IPersistentState<ProductDetails> _product = product;

    public ValueTask CreateOrUpdateProductAsync(ProductDetails productDetails)
    {
        throw new NotImplementedException();
    }

    public ValueTask<int> GetProductAvailibilityAsync() => ValueTask.FromResult(_product.State.Quantity);

    public ValueTask<ProductDetails> GetProductDetailsAsync() => ValueTask.FromResult(_product.State);

    public ValueTask ReturnProductAsync(int quantity)
    {
        throw new NotImplementedException();
    }

    public ValueTask<(bool IsAvailable, ProductDetails ProductDetails)> TryTakeProductAsync(int quantity)
    {
        throw new NotImplementedException();
    }
}
