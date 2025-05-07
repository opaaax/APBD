using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString;
    ICountryService countryService;

    public TripsService(IConfiguration configuration, ICountryService countryService)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        this.countryService = countryService;
    }
    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();
        
        //returns all trips
        string command = "SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    int tripIdOrdinal = reader.GetInt32(idOrdinal);
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        MaxPeople = reader.GetInt32(5),
                        Countries = await countryService.GetCountries(tripIdOrdinal)
                        
                    });
                }
            }
        }
        return trips;
    }

    public async Task<TripDTO> GetTrip(int id)
    {   
        var trip = new TripDTO();
        //returns trips with a given trip id
        const string command = "SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip WHERE IdTrip = @IdTrip";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdTrip", id);
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                await reader.ReadAsync();
                
                int idOrdinal = reader.GetOrdinal("IdTrip");
                int tripIdOrdinal = reader.GetInt32(idOrdinal);

                trip.Id = reader.GetInt32(idOrdinal);
                trip.Name = reader.GetString(1);
                trip.Description = reader.GetString(2);
                trip.StartDate = reader.GetDateTime(3);
                trip.EndDate = reader.GetDateTime(4);
                trip.MaxPeople = reader.GetInt32(5);
                trip.Countries = await countryService.GetCountries(tripIdOrdinal);
            }
        }
        return trip;
    }
    
    public async Task<bool> DoesTripExist(int tripId)
    {
        var trip = new TripDTO();
        //returns 1 if there is a trip with a given id
        const string command = "SELECT 1 FROM Trip WHERE EXISTS(SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip WHERE IdTrip = @IdTrip)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            await conn.OpenAsync();

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                await reader.ReadAsync();
                try
                {
                    var doesTripExist = reader.GetInt32(0);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;

            }
        }
    }
}