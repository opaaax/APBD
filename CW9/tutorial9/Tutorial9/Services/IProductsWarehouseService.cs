using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public interface IProductsWarehouseService
{
    Task<int> PostProductWarehouse(ProductsWarehouseDTO productsWarehouse, int orderId);
    Task<bool> DoesWarehouseExist(int id);
    Task<bool> DoesProductExist(int id);
    Task<KeyValuePair<bool, OrderDTO?>> DoesOrderExist(int idProduct, int amount);
    Task<bool> HasOrderAlreadyBeenCompleted(int idOrder);
    Task<int> PostProductWarehouseProcedure(ProductsWarehouseDTO productsWarehouse);
}