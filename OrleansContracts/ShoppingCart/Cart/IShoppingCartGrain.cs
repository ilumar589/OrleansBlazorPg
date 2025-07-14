using Orleans;
using OrleansContracts.ShoppingCart.Product;

namespace OrleansContracts.ShoppingCart.Cart;

[Alias("OrleansContracts.ShoppingCart.Cart.IShoppingCartGrain")]
public interface IShoppingCartGrain : IGrainWithStringKey
{
    [Alias("AddOrUpdateItemAsync")]
    ValueTask<bool> AddOrUpdateItemAsync(int quantity, ProductDetails product);
    [Alias("RemoveItemAsync")]
    ValueTask RemoveItemAsync(ProductDetails product);
    [Alias("GetAllItemsAsync")]
    ValueTask<HashSet<CartItem>> GetAllItemsAsync();
    [Alias("GetTotalItemsInCartAsync")]
    ValueTask<int> GetTotalItemsInCartAsync();
    [Alias("EmptyCartAsync")]
    ValueTask EmptyCartAsync();
}
