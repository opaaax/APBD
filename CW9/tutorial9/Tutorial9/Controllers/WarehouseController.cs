using Microsoft.AspNetCore.Mvc;
using Tutorial9.Model.DTOs;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IProductsWarehouseService _productsWarehouseService;

    public WarehouseController(IProductsWarehouseService productsWarehouseService)
    {
        _productsWarehouseService = productsWarehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(ProductsWarehouseDTO productsWarehouseDto)
    {
        if (!await _productsWarehouseService.DoesProductExist(productsWarehouseDto.IdProduct))
        {
            return BadRequest("Product doesn't exist");
        }
        if (!await _productsWarehouseService.DoesWarehouseExist(productsWarehouseDto.IdWarehouse))
        {
            return BadRequest("Warehouse doesn't exist");
        }
        if (productsWarehouseDto.Amount <= 0)
        {
            return BadRequest("Warehouse amount must be greater than 0");
        }
        var doesOrderExists =
            await _productsWarehouseService.DoesOrderExist(productsWarehouseDto.IdProduct, productsWarehouseDto.Amount);
        Console.WriteLine(doesOrderExists.Value.IdOrder);
        if (!doesOrderExists.Key)
        {
            return BadRequest("Order doesn't exist");
            
        }
        if (doesOrderExists.Value.CreatedAt.CompareTo(productsWarehouseDto.CreatedAt) < 0)
        {
            return BadRequest("warehouse createdAt should be earlier than order CreatedAt");

        }
        if (await _productsWarehouseService.HasOrderAlreadyBeenCompleted(doesOrderExists.Value.IdOrder))
        {
            return BadRequest("Order already completed");
        }
        
        int returnId = await _productsWarehouseService.PostProductWarehouse(productsWarehouseDto, doesOrderExists.Value.IdOrder);
        return Created("", returnId);
    }
       
}