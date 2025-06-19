

using TrackerService.Domain.Entities;
using TrackerService.Domain.Interface;

namespace TrackerService.Application.Services
{
    public class TrackerServicecls : ITrackerService
    {
        private readonly IPeopleServiceClient _peopleClient;

        public TrackerServicecls(IPeopleServiceClient peopleClient)
        {
            _peopleClient = peopleClient;
        }

        public async Task<List<TrackPerson>> GetAllPeopleAsync(string token)
        {
            return await _peopleClient.GetAllPeopleAsync(token);
        }

        public async Task<TrackPerson?> GetPersonByIdAsync(int id, string token)
        {
            return await _peopleClient.GetPersonByIdAsync(id, token);
        }
    }
}

