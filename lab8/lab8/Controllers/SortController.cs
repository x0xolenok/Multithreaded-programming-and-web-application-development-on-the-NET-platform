using Microsoft.AspNetCore.Mvc;
using lab8.Models;
using System.Globalization;

namespace lab8.Controllers
{
    public class SortController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sort(SortModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                model.OriginalNumbers = model.InputNumbers;
                model.SortedNumbers = model.BubbleSort();
            }
            catch (Exception)
            {
                ModelState.AddModelError("InputNumbers", "Invalid input. Please enter numbers separated by commas.");
                return View("Index", model);
            }

            return View("Result", model);
        }

    }
}