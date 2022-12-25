using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Book.Untils;
using Book.Data;

namespace Book.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHash;
        private readonly ILogger<AdminController> _logger;
        // public List<ApplicationUser> users {get; set;}
        public AdminController(ILogger<AdminController> logger,
                               UserManager<ApplicationUser> userManager, 
                               ApplicationDbContext context, 
                               IPasswordHasher<ApplicationUser> passwordHash )
        {
            _logger = logger;
             _userManager = userManager;
            _context = context;
            _passwordHash = passwordHash;
            // _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Customers(){
            // var usersWithPermission = await _userManager.GetUserAsync(User);
            // var usersWithPermission = _userManager.GetUserAsync(User);
            var usersWithPermission = _userManager.Users;
            return View(usersWithPermission);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeCustomerPassword(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Customers");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCustomerPassword(string id, string password)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = _passwordHash.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Customers");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        // Approve
        public IActionResult GenreApproval()
        {
            IEnumerable<Genre> genres = _context.genres
                                                .Where(t => t.Status == Enums.GenreApproval.pending)
                                                .ToList();
            return View(genres);
        }
        public IActionResult ApproveGenre(int id)
        {
            var genreInDb = _context.genres.SingleOrDefault(t => t.Id == id);
            if (genreInDb is null)
            {
                return BadRequest();
            }

            genreInDb.Status = Enums.GenreApproval.approved;

            Notification noti = new Notification
            {
                Message = "Your genre request for " + genreInDb.Description + " has been approved",
                NotiStatus = Enums.NotiCheck.unSeen
            };
            _context.Add(noti);
            
            _context.SaveChanges();
            return RedirectToAction("GenreList");
            // return View();
        }

        public IActionResult RejectGenre(int id)
        {
            var genreInDb = _context.genres.SingleOrDefault(t => t.Id == id);
            if (genreInDb is null)
            {
                return BadRequest();
            }

            genreInDb.Status = Enums.GenreApproval.rejected;

            Notification noti = new Notification
            {
                Message = "Your genre request for " + genreInDb.Description + " has been rejected",
                NotiStatus = Enums.NotiCheck.unSeen
            };
            _context.Add(noti);

            _context.SaveChanges();
            return RedirectToAction("GenreList");
        }
        [HttpGet]
        public IActionResult GenreList(){
             var genres = _context.genres;

            return View(genres);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var genreInDb = _context.genres.SingleOrDefault(t => t.Id == id);
            if (genreInDb != null)
            {
                var genreView = new Genre(){
                    Id = genreInDb.Id,
                    Description = genreInDb.Description,
                    Status = genreInDb.Status
                };
                return View(genreView);
                
            }else{
                return NotFound();
            }
            
        }

        [HttpPost]
         public async Task<IActionResult> Update(Genre viewGenre){
            if (ModelState.IsValid)
            {
                var genre = new Genre(){
                    Id = viewGenre.Id,
                    Description = viewGenre.Description,
                    Status = viewGenre.Status
                };
                _context.genres.Update(genre);
                _context.SaveChanges();
                TempData["successMessage"] = "Update SuccessFully";
                return RedirectToAction("GenreList");
            }
            else{
                return NotFound();
            }
         }


        [HttpGet]
        public async Task<IActionResult>  Delete(int id)
        {
            var genreInDb = _context.genres.SingleOrDefault(t => t.Id == id);
            if (genreInDb is null)
            {
                return NotFound();
            }
     
            _context.genres.Remove(genreInDb);
            _context.SaveChanges();
            return RedirectToAction("GenreList");
        }
        

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}