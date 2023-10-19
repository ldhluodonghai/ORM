using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Result;
using Service;
using Usermanger.Event.Model;
using Usermanger.EventDomain.Service;

namespace Usermanger.Event.Controllers
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    //[Authorize(Roles ="书记，罗，主任")]
    public class EventController : ControllerBase
    {

        private readonly EventService eventService;
        private readonly ReviewService reviewService;
        private readonly UserEventReviewService userEventReviewService;
        private readonly UserReviewService userReviewService;
        private readonly UserService userService;

        public EventController(EventService eventService, ReviewService reviewService, UserEventReviewService userEventReviewService, UserReviewService userReviewService,
            UserService userService)
        {
            this.eventService = eventService;
            this.reviewService = reviewService;
            this.userEventReviewService = userEventReviewService;
            this.userReviewService = userReviewService;
            this.userService = userService;
        }

        [HttpGet]
        public void Test()
        {
            Console.WriteLine("ldh");
        }

        //添加事件
        [HttpPost]

        public ResultApi AddEvents(string tiles,string descr)
        {
            Events events = new Events();
            events.Name = tiles;
            events.Description = descr;
            int v = eventService.AddEvent(events);
            if (v > 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("事件添加失败");
            }
        }
        //添加部门
        [HttpPost]

        public ResultApi AddReview(string ReviewName,string userName)
        {
            Review review = new Review();
            review.Name= ReviewName;

            int v = reviewService.Add(review);
            int v1 = userReviewService.AddUserReview(userName, ReviewName);

            if (v+v1 > 1)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("事件添加失败");
            }
        }
        //分配任务
        [HttpPost]
        [Filter(ActionName ="luo")]
        public ResultApi AllotEvent(string[] reviewName,string eventName)
        {
            int length = reviewName.Length;
            int v = userEventReviewService.AllotEvent(reviewName, eventName);
            if (v ==length)
            {
                return ResultHelper.Success(v);
            }
            return ResultHelper.Error("分配失败");
        }

        //查看审核任务
        [HttpGet]
        public ResultApi GetReview(string? name)
        {
            if (name == null)
            {
                var value = this.User.FindFirst("id");
                Guid id =new Guid(value!.Value);
                UserEventReview userEventReview = userEventReviewService.FindByAnyIds(id);
                if (userEventReview == null)
                {
                    return ResultHelper.Error("没有任务审核");
                }
                if (userEventReview.IsCheck == 0)
                {
                    Events events = eventService.FindByAnyIds(userEventReview.EventId);
                    return ResultHelper.Success(events);
                }
                return ResultHelper.Error("审核完成");
            }
            global::Model.Entitys.User user = userService.Find(name);
            UserEventReview userEventReview1 = userEventReviewService.FindByAnyIds(user.Id);
            if (userEventReview1 == null)
            {
                return ResultHelper.Error("没有任务审核");
            }
            if (userEventReview1.IsCheck == 0)
            {
                Events events = eventService.FindByAnyIds(userEventReview1.EventId);
                return ResultHelper.Success(events);
            }
            return ResultHelper.Error("审核完成");
        }

    }
}
