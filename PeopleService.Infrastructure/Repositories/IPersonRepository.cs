using PeopleService.Domain.Entities;

namespace PeopleService.Infrastructure.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<Person> AddAsync(Person person);
        Task<bool> UpdateAsync(Person person);
        Task<bool> DeleteAsync(int id);
    }
}