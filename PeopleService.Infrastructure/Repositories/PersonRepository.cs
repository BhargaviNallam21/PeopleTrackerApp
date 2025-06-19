using Microsoft.EntityFrameworkCore;
using PeopleService.Domain.Entities;
using PeopleService.Infrastructure.Data;

namespace PeopleService.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PeopleDbContext _context;

        public PersonRepository(PeopleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task<Person> AddAsync(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            var existing = await _context.People.FindAsync(person.Id);
            if (existing == null) return false;

            existing.FullName = person.FullName;
            existing.Email = person.Email;
            existing.BirthDate = person.BirthDate;
            existing.City = person.City;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null) return false;

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}