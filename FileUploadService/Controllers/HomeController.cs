using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FileUploadService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            string assemblyVersion = version != null ? version.ToString() : "No version defined";
            string content = $"FileUpload-Service Api (version {assemblyVersion})";

            _logger.LogInformation(content);

            return Content(content);
        }
    }
}
