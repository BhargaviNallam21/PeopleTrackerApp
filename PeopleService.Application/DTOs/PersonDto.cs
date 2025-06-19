namespace PeopleService.Application.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
    }
}
