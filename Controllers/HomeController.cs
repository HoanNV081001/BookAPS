using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Book.Models;
using Book.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
 
    public IActionResult Privacy()
    {
        return View();
    }
    public string hoan(){
        return "asdf";
    }
    [HttpGet]
        public IActionResult GenreList()
        {
            var genres = _context.genres;

            return View(genres);
        }

        [HttpGet]
        public IActionResult GenreRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenreRequest(int id, string description)
        { 
          if(description != null)
          {
            Genre genre = new Genre()
             {
              Id = id,
              Description = description,
              Status = Enums.GenreApproval.pending
             };
        _context.Add(genre);
          }
          else
          {
            return RedirectToAction("GenreRequest");
          }
            await _context.SaveChangesAsync();
            return RedirectToAction("GenreList");
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
