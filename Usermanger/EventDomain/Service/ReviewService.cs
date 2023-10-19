using UserManagement.Db;
using Usermanger.Event.Model;

namespace Usermanger.EventDomain.Service
{
    public class ReviewService:ORMSqlHelper<Review>
    {
        public int AddReview(Review e)
        {
            int v = Add(e);
            return v;
        }
    }
}
