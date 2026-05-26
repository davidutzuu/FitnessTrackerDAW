using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Domain;
using FitnessTrackerPAW.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTrackerPAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Pagina 3: Dashboard-ul principal (Accesibil tuturor, dar afiseaza optiuni diferite daca esti logat)
        public IActionResult Index()
        {
            return View();
        }

        // Pagina 4: Formular de logare antrenament - Ruta protejata (Redirect la login pentru anonimi)
        [Authorize]
        [HttpGet]
        public IActionResult LogWorkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogWorkout(WorkoutSessionDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Validare server-side esuata
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = new WorkoutSession
            {
                Date = DateTime.UtcNow,
                DurationInMinutes = model.DurationInMinutes,
                PumpLevel = model.PumpLevel,
                UserId = userId!
            };

            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Pagina 5: Componenta SPA (React) pentru analiza progresului - Ruta protejata
        [Authorize]
        public IActionResult Analytics()
        {
            return View();
        }
    }
}