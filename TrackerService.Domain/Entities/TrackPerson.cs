namespace TrackerService.Domain.Entities
{
    public class TrackPerson
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string City { get; set; } = string.Empty;
    }
}
