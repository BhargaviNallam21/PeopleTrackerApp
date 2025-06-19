using PeopleService.Application.DTOs;
using PeopleService.Domain.Entities;
using PeopleService.Infrastructure.Repositories;

namespace PeopleService.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;

        public PersonService(IPersonRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            var people = await _repo.GetAllAsync();
            return people.Select(p => new PersonDto
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
                BirthDate = p.BirthDate,
                City = p.City
            });
        }

        public async Task<PersonDto?> GetByIdAsync(int id)
        {
            var person = await _repo.GetByIdAsync(id);
            if (person == null) return null;

            return new PersonDto
            {
                Id = person.Id,
                FullName = person.FullName,
                Email = person.Email,
                BirthDate = person.BirthDate,
                City = person.City
            };
        }

        public async Task<PersonDto> CreateAsync(PersonDto person)
        {
            var entity = new Person
            {
                FullName = person.FullName,
                Email = person.Email,
                BirthDate = person.BirthDate,
                City = person.City
            };

            var created = await _repo.AddAsync(entity);
            return new PersonDto
            {
                Id = created.Id,
                FullName = created.FullName,
                Email = created.Email,
                BirthDate = created.BirthDate,
                City = created.City
            };
        }

        public async Task<bool> UpdateAsync(int id, PersonDto person)
        {
            var entity = new Person
            {
                Id = id,
                FullName = person.FullName,
                Email = person.Email,
                BirthDate = person.BirthDate,
                City = person.City
            };

            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
