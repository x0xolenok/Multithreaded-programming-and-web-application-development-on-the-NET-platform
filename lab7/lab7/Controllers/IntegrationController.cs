using lab7.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers
{
    public class IntegrationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(IntegrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            model.Result = model.CalculateTrapezoidalIntegral();
            return View("Result", model);
        }

    }
}