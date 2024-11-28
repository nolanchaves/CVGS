namespace CVGS.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public string Location { get; set; }
        public int MaxRegistrations { get; set; }
        public List<EventRegistration> Registrations { get; set; }
    }

}
