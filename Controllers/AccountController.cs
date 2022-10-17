using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Entities;
using MvcProject.Models;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MvcProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;


        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = DoMD5HashedString(model.Password);

                Entities.User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User Locked.");
                        return View(model);
                    }

                    List<Claim> claims = new();
                    // claims.Add(new Claim("Id",user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                    //null ise ?? boşlık yaz
                    claims.Add(new Claim(ClaimTypes.Name, user.NameSurname ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity identy = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identy);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    // Login olursa (User.Identity.IsAuthenticated =true olur

                    return RedirectToAction("Index", "Home");

                }
                else
                {

                    ModelState.AddModelError("", "Username or password is incoreect.");
                }



            }
            return View(model);
        }

        private string DoMD5HashedString(string saltyThing)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = saltyThing + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                    //Eğer öyle bir kullanıcı varsa hata yı görsün kod daha fazla ilerlemesin
                    return View(model);
                }

                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");


                string hashedPassword = DoMD5HashedString(model.Password);


                Entities.User user = new()
                {
                    Username = model.Username,
                    Password = hashedPassword
                };

                _databaseContext.Users.Add(user);
                //SaveChanges metodu kaydettiği değişiklik sayısını döndürür
                int affectedRowCount = _databaseContext.SaveChanges();
                if (affectedRowCount==0)
                {
                    //Bazen veri tabanında insert yapamayıp hatada vermeyebbilir bunları yakalamak için 
                    /*AddModelError Key =Hangi property ile ilgili, O property altınfaki span da gözükür Messaj*/
                    ModelState.AddModelError("", "User can not be added");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(model);
        }

        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Entities.User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

            //Veri tabanı nesnesini dönmeyişo view data dönüyoruz
            ViewData["Username"] = user.Username;
            ViewData["ProfileImageName"]=user.ProfileImageName;
        }

        [HttpPost]
        //Model olmadığı durumda mvc html deki name atributu na bakar mvc 
        public IActionResult ProfileChangeUserName( [Required] [StringLength(50)] string? username)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Entities.User user= _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                user.Username = username;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();
            return View("Profile");//Geriye donerken ProfileChangeFullName arama profile dön
        }

        public IActionResult ProfileChangePassword([Required] [MinLength(6)] [MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {

                Guid userid=new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Entities.User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                string hashedPAssword = DoMD5HashedString(password);

                user.Password = hashedPAssword;
                _databaseContext.SaveChanges();

                ViewData["ProfileChangePasswordResult"]="PasswordChanged";

            }
            ProfileInfoLoader();
            return View("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]

        public IActionResult ProfileChangeImage([Required] IFormFile file )
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Entities.User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                string fileName = $"pig_{userid}.png";
                Stream stream = new FileStream($"wwwroot/uploads/{fileName}", FileMode.OpenOrCreate);
                file.CopyTo(stream);
                stream.Close();
                stream.Dispose();

                user.ProfileImageName=fileName;
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Profile));
            }

            ProfileInfoLoader();
            return View("Profile");
        }
    }
}
