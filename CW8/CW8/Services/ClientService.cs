using Microsoft.AspNetCore.Components.Sections;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientService : IClientService
{
    ITripsService _tripsService;
    private readonly string? _connectionString;

        
    public ClientService(ITripsService tripsService, IConfiguration configuration)
    {
        _tripsService = tripsService;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<List<TripClientInfoDTO>> GetTripsByClientId(int clientId)
    {
        var trips = new List<TripClientInfoDTO>();
        
        //Client_Trip table, with clientId
        string command = "SELECT IdClient, IdTrip, RegisteredAt, PaymentDate FROM Client_Trip Where IdClient = @IdClient";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinalTrip = reader.GetOrdinal("IdTrip");
                    int tripIdOrdinal = reader.GetInt32(idOrdinalTrip);
                    trips.Add(new TripClientInfoDTO
                    {
                        Trip = await _tripsService.GetTrip(tripIdOrdinal),
                        RegisteredAt = reader.GetInt32(2),
                        PaymentDate = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                    });
                }
            }
        }
        return trips;
    }

    public async Task<bool> DoesClientExist(int clientId)
    {
        //throws an exception if client does not exist
        string command = "SELECT 1 FROM Client_Trip WHERE IdClient = @IdClient";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                await reader.ReadAsync();
                try
                {
                    var temp = reader.GetInt32(0);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }
        
    }

    public async Task<int> CreateClient(ClientDTO client)
    {
        //inserts a new client into the db
        string command = "INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) output INSERTED.IdClient Values(@FirstName ,@LastName, @Email, @Telephone, @Pesel)";
        int createdId;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel);
            await conn.OpenAsync();
            createdId = (int) cmd.ExecuteScalar();
        }
        return createdId;
    }

    public async Task PutTrip(int clientId, int tripId)
    {
        //inserts into Client_Trip
        string command = "INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@IdClient, @IdTrip, @RegisteredAt)";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            int date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            cmd.Parameters.AddWithValue("@RegisteredAt", date);
            await conn.OpenAsync();
            cmd.ExecuteNonQuery();
        }
    }

    public async Task DeleteTrip(int clientId, int tripId)
    {
        //inserts into Client_Trip
        string command = "DELETE FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            await conn.OpenAsync();
            cmd.ExecuteNonQuery();
        }
    }

    public async Task<bool> DoesRegistrationExist(int clientId, int tripId)
    {
        string command = "SELECT 1 FROM Client_Trip Where IdClient = @IdClient and IdTrip = @IdTrip";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                await reader.ReadAsync();
                try
                {
                    var temp = reader.GetInt32(0);
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