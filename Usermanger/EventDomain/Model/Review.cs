namespace Usermanger.Event.Model
{
    public class Review
    {
        public Guid  Id { get;private set; } 
        public string Name { get; set; }

        public Review (string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Review(Guid id)
        {
            Id = id;
        }
        public Review() {
            Id = Guid.NewGuid();
        }
    }
}
