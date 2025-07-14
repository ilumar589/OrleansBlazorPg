using Orleans.Concurrency;
using OrleansContracts.ShoppingCart.Product;

namespace OrleansServer.ShoppingCart.Product;

[Reentrant]
public sealed class InventoryGrain(
    [PersistentState(
        stateName: "Inventory",
        storageName: "shopping-cart")]
    IPersistentState<HashSet<string>> productIdsState) : Grain, IInventoryGrain
{
    private readonly Dictionary<string, ProductDetails> _productCache = [];

    public override Task OnActivateAsync(CancellationToken _) => PopulateProductCacheAsync().AsTask();

    public async ValueTask AddOrUpdateProductAsync(ProductDetails product)
    {
        productIdsState.State.Add(product.Id);
        _productCache[product.Id] = product;

        await productIdsState.WriteStateAsync();
    }

    public ValueTask<HashSet<ProductDetails>> GetAllProductsAsync() => ValueTask.FromResult(_productCache.Values.ToHashSet());

    public async ValueTask RemoveProductAsync(string productId)
    {
        productIdsState.State.Remove(productId);
        _productCache.Remove(productId);

        await productIdsState.WriteStateAsync();
    }

    private async ValueTask PopulateProductCacheAsync()
    {
        if (productIdsState is not { State.Count: > 0 }) return;

        await Parallel.ForEachAsync(productIdsState.State, async (productId, _) =>
        {
            var productGrain = GrainFactory.GetGrain<IProductGrain>(productId);
            _productCache[productId] = await productGrain.GetProductDetailsAsync();
        });
    }
}
