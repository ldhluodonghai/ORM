using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys.Dto;
using Model.Result;
using UserManagement.Dao.ESModel;
using UserManagement.ServiceDev.ES;

namespace Usermanger.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleService peopleService;

        public PeopleController(PeopleService peopleService)
        {
            this.peopleService = peopleService;
        }
        [HttpPost]
        public ResultApi GetPeopleKeySearch(PeopleDto peopleDto)
        {
            IEnumerable<People> enumerable = peopleService.GetPeoplesKeySearch(peopleDto);
            return ResultHelper.Success(enumerable);
        }
        [HttpGet]
        public ResultApi GetPeoples()
        {
            return ResultHelper.Success(peopleService.GetAll().ToList());
        }
        [HttpPost]
        public ResultApi GetPeopleById(string id)
        {
            return ResultHelper.Success(peopleService.GetById(id));
        }
        
        [HttpPost]
        public ResultApi AddPeople(People people)
        {
            Nest.IndexResponse indexResponse = peopleService.Create(people);
            return ResultHelper.Success(indexResponse);
        }
        [HttpPost]
        public ResultApi AddPeople1(People people)
        {
            Nest.IndexResponse indexResponse = peopleService.Create1(people);
            return ResultHelper.Success(indexResponse);
        }
    }
}
