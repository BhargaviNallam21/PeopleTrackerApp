using System.ComponentModel.DataAnnotations;

namespace PeopleService.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
