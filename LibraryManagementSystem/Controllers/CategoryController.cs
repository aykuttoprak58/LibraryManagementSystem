using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemUI.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult GetAllCategoriesUI()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategoryUI(string test)
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategoryUI()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateCategoryUI(string test)
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateCategoryUI()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteCategoryUI()
        {
            return View();
        }
    }
}
