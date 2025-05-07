using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientService
{
    Task<List<TripClientInfoDTO>> GetTripsByClientId(int clientId);
    Task<bool> DoesClientExist(int clientId);
    Task<int> CreateClient(ClientDTO client);
    Task PutTrip(int clientId, int tripId);
    Task DeleteTrip(int clientId, int tripId);
    Task<bool> DoesRegistrationExist(int clientId, int tripId);

}