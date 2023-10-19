using Service;
using UserManagement.Db;
using Usermanger.EventDomain.Model;

namespace Usermanger.EventDomain.Service
{
    public class UserReviewService : ORMSqlHelper<UserReview>
    {
        private readonly UserService userService;
        private readonly ReviewService reviewService;

        public UserReviewService(UserService userService, ReviewService reviewService)
        {
            this.userService = userService;
            this.reviewService = reviewService;
        }

        public int AddUserReview(string userName,string reviewNaame)
        {
            global::Model.Entitys.User user = userService.Find(userName);
            Event.Model.Review review = reviewService.FindByName(reviewNaame);
            UserReview userReview = new UserReview();
            userReview.UserId = user.Id;
            userReview.ReviewId=review.Id;
            int v = Add(userReview);
            return v;
        }
    }
}
