using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Entities;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        private readonly IMapper _mapper;

        public MemberController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberListPartial()
        {
            List<UserViewModel> users = _databaseContext.Users.ToList().Select(u=>_mapper.Map<UserViewModel>(u)).ToList();  

            return PartialView("_MemberListPartial",users);
        }
    }
}
