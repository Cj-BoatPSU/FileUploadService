using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using FileUploadService.Models;
using FileUploadService.Models.ServiceModels;
using FileUploadService.Models.DBModels;
using FileUploadService.Services;
using static FileUploadService.Models.ServiceModels.FileModel;
using FileUploadService.Context;

namespace FileUploadService.Controllers
{
    [Area("api")]
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
            try
            {
                if (req.File == null)
                {
                    return StatusCode(400, new ResponseModel()
                    {
                        status = 400,
                        errors = new ExceptionModel() { message = "File must be have." }
                    });
                }

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var handler = new JwtSecurityTokenHandler();
                var token_Payload = handler.ReadJwtToken(accessToken).Payload;
                int? _accountId = int.Parse(token_Payload.Sub);
                req.AccountId = _accountId;


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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfo(int? fileId)
        {
            try
            {
                if (!fileId.HasValue)
                {
                    return StatusCode(400, new ResponseModel()
                    {
                        status = 400,
                        errors = new ExceptionModel() { message = "FileId must be have value." }
                    });
                }

                FileTable res = await _file_Service.GetFileInfo(fileId.Value);

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DownloadFile([FromBody] DownloadFile_Req req)
        {
            try
            {
                if (!req.FileId.HasValue)
                {
                    return StatusCode(400, new ResponseModel()
                    {
                        status = 400,
                        errors = new ExceptionModel() { message = "FileId must be have value." }
                    });
                }

                DownloadFile_Response _res = await _file_Service.DownloadFile(req.FileId.Value);
                var res = File(_res.Content, _res.ContentType, Path.GetFileName(_res.FilePath));
                res.FileDownloadName = _res.FileName;

                return res;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFile_Req req)
        {
            try
            {
                if (!req.FileId.HasValue)
                {
                    return StatusCode(400, new ResponseModel()
                    {
                        status = 400,
                        errors = new ExceptionModel() { message = "FileId must be have value." }
                    });
                }

                await _file_Service.DeleteFile(req.FileId.Value);

                return StatusCode(200, new ResponseModel() { status = 200, data = null });
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
