using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using FileUploadService.Models;
using FileUploadService.Models.ServiceModels;
using FileUploadService.Models.DBModels;
using FileUploadService.Services;

namespace FileUploadService.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private IFile_Service _file_Service;

        public FileController(ILogger<FileController> logger, IFile_Service file_Service)
        {
            _logger = logger;
            _file_Service = file_Service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] FileModel.UploadFile_Req req)
        {
            int? _accountId = null;
            try
            {

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var handler = new JwtSecurityTokenHandler();
                var token_Payload = handler.ReadJwtToken(accessToken).Payload;
                _accountId = int.Parse(token_Payload.Sub);

                FileTable res = await _file_Service.UploadFile(req);

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
                });
            }
        }
    }
}
