using FileUploadService.Models.ServiceModels;
using FileUploadService.Models;
using FileUploadService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadService.Controllers
{
    [Area("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private IAuthentication_Service _authentication_Service;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthentication_Service authentication_Service)
        {
            _logger = logger;
            _authentication_Service = authentication_Service;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login_Req req)
        {
            Login_Res res = new Login_Res();
            try
            {
                if (string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
                {
                    return StatusCode(400, new ResponseModel()
                    {
                        status = 400,
                        errors = new ExceptionModel() { message = "Please fill in the required information." }
                    });
                }

                res = _authentication_Service.Login(req.Username, req.Password);

                return StatusCode(200, new ResponseModel() { status = 200, data = res });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel()
                {
                    status = 500,
                    errors = new ExceptionModel()
                    {
                        message = ex.Message,
                    }
                }
                );
            }
        }
    }
}
