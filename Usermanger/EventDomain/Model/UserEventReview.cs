namespace Usermanger.Event.Model
{
    public class UserEventReview
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public Guid ReviewId { get; set; }
        public int IsCheck { get; set; }

        public UserEventReview(Guid id, Guid userId, Guid eventId, Guid revewId, int isCheck)
        {
            Id = id;
            UserId = userId;
            EventId = eventId;
            ReviewId = revewId;
            IsCheck = isCheck;
        }
        public UserEventReview() 
        {
            Id = Guid.NewGuid();
        }
    }
}
