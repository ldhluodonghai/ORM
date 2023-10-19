namespace Usermanger.EventDomain.Model
{
    public class UserReview
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ReviewId { get; set; }

        public UserReview(Guid id, Guid userId, Guid reviewId)
        {
            Id = id;
            UserId = userId;
            ReviewId = reviewId;
        }

        public UserReview()
        {
            Id = Guid.NewGuid();
        }

    }
}
