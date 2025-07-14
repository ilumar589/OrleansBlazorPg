using OrleansContracts.ShoppingCart.Product;

namespace OrleansServer.ShoppingCart.Product;

public sealed class InventoryGrain(
    [PersistentState(
        stateName: "Inventory",
        storageName: "shopping-cart")]
    IPersistentState<HashSet<string>> state) : Grain, IInventoryGrain
{
    private readonly Dictionary<string, ProductDetails> _productCache = [];

    public override Task OnActivateAsync(CancellationToken _) => PopulateProductCacheAsync().AsTask();

    public ValueTask AddOrUpdateProductAsync(ProductDetails productDetails)
    {
        throw new NotImplementedException();
    }

    public ValueTask<HashSet<ProductDetails>> GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveProductAsync(string productId)
    {
        throw new NotImplementedException();
    }

    private async ValueTask PopulateProductCacheAsync()
    {
        if (state is not { State.Count: > 0 }) return;

        await Parallel.ForEachAsync(state.State, async (productId, _) =>
        {
            var productGrain = GrainFactory.GetGrain<IProductGrain>(productId);
            _productCache[productId] = await productGrain.GetProductDetailsAsync();
        });
    }
}
