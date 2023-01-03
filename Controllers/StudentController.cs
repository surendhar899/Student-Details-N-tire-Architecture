using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentBAL;
using StudentDAL.Models;
using System.Security.Claims;

namespace Student_Details_N_tire.Controllers
{
    //changes
    [Authorize]
    public class StudentController : Controller
    {
        private readonly StudentInterface _studentInterface;
        public StudentController(StudentInterface studentInterface)
        {
            _studentInterface = studentInterface;
        }
        [AllowAnonymous]
        public ActionResult login(login login)
        {
            var user = login.UserName;
            var pass=login.Password;
            var std = _studentInterface.login(login);
            var username = std[0].UserName;
            var password = std[0].Password;
            if(username==user && password==pass)
            {
                List<Claim> claims = new List<Claim>()
          {
              new Claim(ClaimTypes.NameIdentifier, login.UserName),
              new Claim("OtherProperties","Example Role")
          };
                ClaimsIdentity claimsIdentity=new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties=new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = login.KeepLoggedIn
                };
             HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(claimsIdentity),properties);
                return RedirectToAction("Index", "Home");
            };
            TempData["AlertMessage"] = "Please Enter correct Or Password";
            return RedirectToAction("Login","Home");
        }
        
        // GET: StudentController
     
        public ActionResult Search(string SearchText)
        {
            if(SearchText!=null)
            {
                var std = _studentInterface.search(SearchText);
                if (std.Count == 0)
                {
                    TempData["AlertMessage1"] = "Name Not Found";
                    return RedirectToAction("Index");
                }
                ViewBag.count = std.Count();
                ViewBag.department = SearchText;
                return View(std);
            }
            TempData["AlertMessage2"] = "Please Enter The Name";
            return RedirectToAction("Index");

        }
    
        public ActionResult Index()
        {
            var std=_studentInterface.GetStudents();
            return View(std);
        }

        // GET: StudentController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            StudentModel std1 = new StudentModel();
              std1.Id = id;
            var students = _studentInterface.DetailStudent(id);
            return PartialView("_Details",students);
        }

        // GET: StudentController/Create
  
        public ActionResult Create()

        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem{Text="CSE",Value="CSE"},
                 new SelectListItem{Text="IT",Value="IT"},
                 new SelectListItem{Text="MECH",Value="MECH"}
            };
            ViewBag.department = list;
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(StudentModel studentModel)
        {
            if(ModelState.IsValid)
            {
                var std = _studentInterface.InsertStudent(studentModel);
                TempData["AlertMessage"] = "Created Successfully....!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create",studentModel);

        }

        // GET: StudentController/Edit/5

        public ActionResult Edit(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem{Text="CSE",Value="CSE"},
                 new SelectListItem{Text="IT",Value="IT"},
                 new SelectListItem{Text="MECH",Value="MECH"}
            };
            ViewBag.department = list;
            var std= _studentInterface.EditStudent(id);
            return View(std);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Edit(StudentModel studentModel)
        {
            var std = _studentInterface.UpdateStudent(studentModel);
            TempData["AlertMessage"] = "Edited Successfully....!";
            return RedirectToAction("index");
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            var std=_studentInterface.DeleteStudent(id);
            TempData["AlertMessage"] = "Deleted Successfully....!";
            return RedirectToAction("Index");
        }

       
    }
}
