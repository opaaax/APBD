using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ITripsService _tripsService;
    
    public ClientsController(IClientService clientService, ITripsService tripsService)
    {
        _clientService = clientService;
        _tripsService = tripsService;
    }
    
    //returns a list of trips made by a client
    [HttpGet("{id:int}/trips")]
    public async Task<IActionResult> GetTrips(int id)
    {
        if (!await _clientService.DoesClientExist(id))
        {
            return NotFound();
        }
        var trips = await _clientService.GetTripsByClientId(id);
        return Ok(trips);
    }

    //creates a new client
    [HttpPost]
    public async Task<IActionResult> CreateClient(ClientDTO client)
    {
        int returnId = 0;
        try
        {
            returnId = await _clientService.CreateClient(client);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return Created("", returnId);
    }

    [HttpPut("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> PutClient(int clientId, int tripId)
    {
        if (!(await _clientService.DoesClientExist(clientId) && await _tripsService.DoesTripExist(tripId)))
        {
            return Conflict("Client or Trip doesn't exist");
        }
        try
        {
            await _clientService.PutTrip(clientId, tripId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Created();
    }

    [HttpDelete("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> DeleteClientTrip(int clientId, int tripId)
    {
        if (!await _clientService.DoesRegistrationExist(clientId, tripId))
        {
            return NotFound("Registration doesn't exist");
        }

        try
        {
            await _clientService.DeleteTrip(clientId, tripId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok("Trip deleted");
    }
    
}