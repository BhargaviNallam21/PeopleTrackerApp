using TrackerService.Domain.Entities;

namespace TrackerService.Domain.Interface
{
    public interface IPeopleServiceClient
    {
        Task<List<TrackPerson>> GetAllPeopleAsync(string token);
        Task<TrackPerson?> GetPersonByIdAsync(int id, string token);
    }
}
