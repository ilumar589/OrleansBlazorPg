using Orleans.Concurrency;
using OrleansContracts.ShoppingCart.Cart;
using OrleansContracts.ShoppingCart.Product;

namespace OrleansServer.ShoppingCart.Cart;

[Reentrant]
public sealed class ShoppingCartGrain(
    [PersistentState(
            stateName: "ShoppingCart",
            storageName: "shopping-cart")]
    IPersistentState<Dictionary<string, CartItem>> cart) : Grain, IShoppingCartGrain
{
    public async ValueTask<bool> AddOrUpdateItemAsync(int quantity, ProductDetails product)
    {
        var products = GrainFactory.GetGrain<IProductGrain>(product.Id);

        int? adjustedQuantity = null;

        if (cart.State.TryGetValue(product.Id, out var existingItem))
        {
            adjustedQuantity = quantity - existingItem.Quantity;
        }

        var (isAvailable, claimedProduct) = await products.TryTakeProductAsync(adjustedQuantity ?? quantity);

        if (isAvailable && claimedProduct is not null)
        {
            var item = ToCartItem(quantity, claimedProduct);
            cart.State[claimedProduct.Id] = item;

            await cart.WriteStateAsync();
            return true;
        }

        return false;
    }

    public async ValueTask EmptyCartAsync()
    {
        cart.State.Clear();
        await cart.ClearStateAsync();
    }

    public ValueTask<HashSet<CartItem>> GetAllItemsAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<int> GetTotalItemsInCartAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveItemAsync(ProductDetails product)
    {
        throw new NotImplementedException();
    }

    private CartItem ToCartItem(int quantity, ProductDetails product)
    {
        return new CartItem
        {
            UserId = this.GetPrimaryKeyString(),
            Quantity = quantity,
            Product = product
        };
    }
}
