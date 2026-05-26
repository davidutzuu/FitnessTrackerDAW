using Microsoft.AspNetCore.Mvc;
using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Application.DTOs;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // Necesar pentru a extrage ID-ul utilizatorului

namespace FitnessTrackerPAW.Controllers
{
    [Authorize] // Doar utilizatorii autentificati pot vedea aceasta pagina
    public class NutritionController : Controller
    {
        private readonly ISupplementService _supplementService;

        public NutritionController(ISupplementService supplementService)
        {
            _supplementService = supplementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Aflam codul secret (ID-ul) al utilizatorului logat in acest moment
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // 2. Aducem datele din service
            var allSupplements = await _supplementService.GetAllSupplementsAsync();

            // 3. FILTRUL MAGIC: Pastram in lista DOAR elementele care apartin acestui cont
            var mySupplements = allSupplements.Where(s => s.UserId == userId).ToList();

            // 4. Calculam totalurile strict pentru utilizatorul curent
            ViewBag.TotalCalories = mySupplements.Sum(s => s.CaloriesPerServing);
            ViewBag.TotalProtein = mySupplements.Sum(s => s.ProteinPerServing);

            // Trimitem catre interfata doar lista filtrata
            return View(mySupplements);
        }

        [HttpPost]
        public async Task<IActionResult> AddMacro(string name, int calories, int protein, bool isMassGainer)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Index");
            }

            var dto = new SupplementDto
            {
                Name = name,
                CaloriesPerServing = calories,
                ProteinPerServing = protein,
                IsMassGainer = isMassGainer
            };

            // 1. Extragem ID-ul utilizatorului logat in browser
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // 2. Transmitem atat datele cat si utilizatorul catre baza de date
            await _supplementService.AddSupplementAsync(dto, userId);

            return RedirectToAction("Index");
        }
    }
}