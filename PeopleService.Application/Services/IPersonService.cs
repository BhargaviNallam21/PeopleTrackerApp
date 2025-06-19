using PeopleService.Application.DTOs;

namespace PeopleService.Application.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllAsync();
        Task<PersonDto?> GetByIdAsync(int id);
        Task<PersonDto> CreateAsync(PersonDto person);
        Task<bool> UpdateAsync(int id, PersonDto person);
        Task<bool> DeleteAsync(int id);
    }
}
