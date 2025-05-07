using System.Configuration;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Tutorial8.Services;

public class CountryService : ICountryService
{

    private readonly string? _connectionString;

    public CountryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<List<CountryDTO>> GetCountries(int idTrip)
    {
        var countries = new List<CountryDTO>();
        //returns names of countries with the given trip id
        string command = "Select Name from Country JOIN Country_Trip ON Country.IdCountry = Country_Trip.IdCountry WHERE IdTrip = @IdTrip";
        
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, connection))
        {
            cmd.Parameters.AddWithValue("@IdTrip", idTrip);
            await connection.OpenAsync();
        
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    countries.Add(new CountryDTO()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }
        }
        return countries;
    }
}