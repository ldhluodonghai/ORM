using Model.Result;
using UserManagement.Db;
using Usermanger.Event.Model;

namespace Usermanger.EventDomain.Service
{
    public class UserEventReviewService:ORMSqlHelper<UserEventReview>
    {
        private readonly EventService eventService;
        private readonly ReviewService reviewService;
        private readonly UserReviewService userReviewService;

        public UserEventReviewService(EventService eventService, ReviewService reviewService, UserReviewService userReviewService)
        {
            this.eventService = eventService;
            this.reviewService = reviewService;
            this.userReviewService = userReviewService;
        }

        public int AddUserEventReview(UserEventReview userEventReview)
        {
            int v = Add(userEventReview);
            return v;
        }
        public int AllotEvent(string[] reviewName, string titleName)
        {
            int count = 0;
            foreach(var review in reviewName)
            {

                Events events = eventService.FindByName(titleName);
                Review review1 = reviewService.FindByName(review);
                Model.UserReview userReview = userReviewService.FindByAnyIds(review1.Id);
                UserEventReview userEventReview = new UserEventReview();
                userEventReview.UserId=userReview.UserId;
                userEventReview.ReviewId = review1.Id;
                userEventReview.EventId = events.Id;
                userEventReview.IsCheck = 0;
                int v = Add(userEventReview);
                count += v;
            }
            return count;
        }
    }
}
