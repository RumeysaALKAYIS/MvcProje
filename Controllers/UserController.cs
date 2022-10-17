using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Entities;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class UserController: Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public UserController(DatabaseContext databaseContext ,IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            //Database listesi dönmemek için
           // List<User> users = _databaseContext.Users.ToList();
           
            List<UserViewModel> model=_databaseContext.Users.ToList().Select(x=>_mapper.Map<UserViewModel>(x)).ToList();
             

           // _databaseContext.Users.Select(u => new UserModel { Id = u.Id, NameSurname = u.NameSurname }).toList();

            //AutoMaher




            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(u=>u.Username.ToLower()==model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exisiting.");
                    return View(model);
                }

                User user=_mapper.Map<User>(model);
                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public IActionResult Edit(Guid id)
        {
            //Datalar dolu gelsin diye
            User user = _databaseContext.Users.Find(id);
            EditUserModel model =_mapper.Map<EditUserModel>(user);
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Guid id,EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(u=>u.Username.ToLower()==model.Username.ToLower() && u.Id!=id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists");
                }

                User user = _databaseContext.Users.Find(id);
                _mapper.Map(model,user);//modeldeki değerleri user a at 
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            User user = _databaseContext.Users.Find(id);

            if (user != null)
            {

                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();
            }


            return RedirectToAction(nameof(Index));
        }

    }

}
