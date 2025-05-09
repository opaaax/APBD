using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public class ProductsWarehouseService : IProductsWarehouseService
{
    private readonly string? _connectionString;

    public ProductsWarehouseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }
    
    public async Task<int> PostProductWarehouse(ProductsWarehouseDTO productsWarehouse, int idOrder)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand cmd = new SqlCommand();
        
        cmd.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        cmd.Transaction = transaction as SqlTransaction;


        try
        {
            cmd.CommandText = "UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
            cmd.Parameters.AddWithValue("@Idorder", idOrder);
            cmd.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);

            await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
            cmd.Parameters.AddWithValue("IdProduct", productsWarehouse.IdProduct);

            decimal productPrice =(decimal) await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            cmd.CommandText =
                "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) output INSERTED.IdProductWarehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
            cmd.Parameters.AddWithValue("@IdWarehouse", productsWarehouse.IdWarehouse);
            cmd.Parameters.AddWithValue("@IDProduct", productsWarehouse.IdProduct);
            cmd.Parameters.AddWithValue("@IdOrder", idOrder);
            cmd.Parameters.AddWithValue("@Amount", productsWarehouse.Amount);
            cmd.Parameters.AddWithValue("@Price", productPrice * productsWarehouse.Amount);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            int returnId = (int)await cmd.ExecuteScalarAsync();

            await transaction.CommitAsync();
            return returnId;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> PostProductWarehouseProcedure(ProductsWarehouseDTO productsWarehouse)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        command.CommandText = "AddProductToWarehouse";
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.AddWithValue("@IdWarehouse", productsWarehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IDProduct", productsWarehouse.IdProduct);
        command.Parameters.AddWithValue("@Amount", productsWarehouse.Amount);
        command.Parameters.AddWithValue("@CreatedAt", productsWarehouse.CreatedAt);

        int returnId = (int)await command.ExecuteScalarAsync();
        return returnId;
    }

    public async Task<bool> DoesWarehouseExist(int id)
    {
        string command = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, connection))
        {
            cmd.Parameters.AddWithValue("@IdWarehouse", id);
            await connection.OpenAsync();

            try
            {
                cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }

    public async Task<bool> DoesProductExist(int id)
    {
        string command = "SELECT 1 FROM Product WHERE IdProduct = @IdProduct";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, connection))
        {
            cmd.Parameters.AddWithValue("@IdProduct", id);
            await connection.OpenAsync();
            
            int val = (int) cmd.ExecuteScalar();
            
            if (val != 1)
            {
                return false;
            }
            return true;
        }
    }

    public async Task<KeyValuePair<bool, OrderDTO?>> DoesOrderExist(int idProduct, int amount)
    {
        string command = "SELECT IdOrder, CreatedAt FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount";
        
        OrderDTO order = new OrderDTO();
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, connection))
        {
            cmd.Parameters.AddWithValue("@IdProduct", idProduct);
            cmd.Parameters.AddWithValue("@Amount", amount);
            await connection.OpenAsync();
            try
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        order.IdOrder = reader.GetInt32(0);
                        order.CreatedAt = reader.GetDateTime(1);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error: " + sqlEx.Message);
                
            }

            if (order.IdOrder == 0)
            {
                return new KeyValuePair<bool, OrderDTO?>(false, order);
            }
            return new KeyValuePair<bool, OrderDTO?>(true, order);
        }
    }

    public async Task<bool> HasOrderAlreadyBeenCompleted(int idOrder)
    {
        string command = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, connection))
        {
            cmd.Parameters.AddWithValue("@IdOrder", idOrder);
            await connection.OpenAsync();
            
            try
            {
                var val = (int) cmd.ExecuteScalar();
                Console.WriteLine(val);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}