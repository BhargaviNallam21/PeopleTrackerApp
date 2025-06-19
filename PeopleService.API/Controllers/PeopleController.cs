using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleService.Application.DTOs;
using PeopleService.Application.Services;

namespace PeopleService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _service;

        public PeopleController(IPersonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _service.GetByIdAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PersonDto person) => Ok(await _service.CreateAsync(person));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PersonDto person)
        {
            var result = await _service.UpdateAsync(id, person);
            return result ? Ok("Updated") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok("Deleted") : NotFound();
        }
    }
}

