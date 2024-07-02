using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemUI.Controllers
{
    public class WriterControllerUI : Controller
    {
        [HttpGet]
        public IActionResult GetAllWritersUI()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateWriterUI()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateWriterUI(string test)
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteWriterUI()
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateWriterUI()
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateWriterUI(string test)
        {
            return View();
        }
    }
}
