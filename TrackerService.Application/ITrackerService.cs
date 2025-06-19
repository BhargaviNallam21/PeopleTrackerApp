using TrackerService.Domain.Entities;

namespace TrackerService.Application
{
    public interface ITrackerService
    {
        Task<List<TrackPerson>> GetAllPeopleAsync(string token);
        Task<TrackPerson?> GetPersonByIdAsync(int id, string token);
    }
}
