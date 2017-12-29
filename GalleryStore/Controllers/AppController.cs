using Microsoft.AspNetCore.Mvc;
using GalleryStore.Data;
using GalleryStore.ViewModels;
using GalleryStore.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleryStore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IGalleryStoreRepository _repository;

        public AppController(IMailService mailService, IGalleryStoreRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();

        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactVewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("teresitka_91@abv.bg"
                    , model.Subject
                    , $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            else
            {
                //show error
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }
    }
}
