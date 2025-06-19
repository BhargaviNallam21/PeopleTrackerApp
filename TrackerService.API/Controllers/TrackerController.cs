using Microsoft.AspNetCore.Mvc;
using TrackerService.Application;
using TrackerService.Domain.Entities;

namespace TrackerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerService _trackerService;

        public TrackerController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        [HttpGet("people")]
        public async Task<ActionResult<List<TrackPerson>>> GetAllPeople()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var people = await _trackerService.GetAllPeopleAsync(token);
            return Ok(people);
        }

        [HttpGet("people/{id}")]
        public async Task<ActionResult<TrackPerson>> GetPerson(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var person = await _trackerService.GetPersonByIdAsync(id, token);
            if (person == null)
                return NotFound();

            return Ok(person);
        }
    }
}

