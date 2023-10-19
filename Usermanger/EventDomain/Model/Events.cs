namespace Usermanger.Event.Model
{
    public class Events
    {
        public Guid Id { get;private set; }
        public string Name { get;  set; }
        public string Description { get; set; }
        public String Created { get; set; } = DateTime.Now.ToString("D");

        public Events(Guid id)
        {
            Id = id;
        }
        public Events()
        {
            Id = Guid.NewGuid();
            
        }
        public Events(Guid id, string title, string description, string created)
        {
            Id = id;
            Name = title;
            Description = description;
            Created = created;
        }
    }
}
