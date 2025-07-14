using OrleansContracts.ShoppingCart.Product;

namespace OrleansServer.ShoppingCart.Product;

public sealed class ProductGrain(
    [PersistentState(
        stateName: "Product",
        storageName: "shopping-cart")]
    IPersistentState<ProductDetails> productState) : Grain, IProductGrain
{

    private readonly IPersistentState<ProductDetails> _productState = productState;

    public ValueTask CreateOrUpdateProductAsync(ProductDetails productDetails) => UpdateStateAsync(productDetails);

    public ValueTask<int> GetProductAvailibilityAsync() => ValueTask.FromResult(_productState.State.Quantity);

    public ValueTask<ProductDetails> GetProductDetailsAsync() => ValueTask.FromResult(_productState.State);

    public ValueTask ReturnProductAsync(int quantity) => UpdateStateAsync(_productState.State with
    {
        Quantity = _productState.State.Quantity + quantity
    });

    public async ValueTask<(bool IsAvailable, ProductDetails? ProductDetails)> TryTakeProductAsync(int quantity)
    {
        if (_productState.State.Quantity < quantity) return (false, null);

        var updatedState = _productState.State with
        {
            Quantity = _productState.State.Quantity - quantity
        };

        await UpdateStateAsync(updatedState);

        return (true, _productState.State);
    }


    private async ValueTask UpdateStateAsync(ProductDetails product)
    {
        var oldCategory = _productState.State.Category;

        _productState.State = product;
        await _productState.WriteStateAsync();

        var inventoryGrain = GrainFactory.GetGrain<IInventoryGrain>(_productState.State.Category.ToString());
        await inventoryGrain.AddOrUpdateProductAsync(product);

        if (oldCategory != _productState.State.Category)
        {
            // if category changed, remove the product from the old category grain
            var oldCategoryGrain = GrainFactory.GetGrain<IInventoryGrain>(oldCategory.ToString());
            await oldCategoryGrain.RemoveProductAsync(product.Id);
        }
    }
}
