using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ICountryService
{
    Task<List<CountryDTO>> GetCountries(int idTrip);
}