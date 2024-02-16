using FileUploadService.Context;
using FileUploadService.Models.DBModels;
using Microsoft.AspNetCore.StaticFiles;
using static FileUploadService.Models.ServiceModels.FileModel;

namespace FileUploadService.Services
{
    public interface IFile_Service
    {
        public Task<FileTable> UploadFile(UploadFile_Req req);
        public Task<FileTable> GetFileInfo(int fileId);
        public Task<DownloadFile_Response> DownloadFile(int fileId);
        public Task DeleteFile(int fileId);
    }
    public class File_Service : IFile_Service
    {
        public async Task<FileTable> UploadFile(UploadFile_Req req)
        {
            DateTime TxTimestamp = DateTime.UtcNow;
            FileTable new_file = new FileTable();
            string filename = "";

            try
            {
                string extension = "." + req.File.FileName.Split('.')[req.File.FileName.Split('.').Length - 1];
                string filename_Gen = GenerateUniqueFileName();
                filename = filename_Gen + extension;

                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                string exactpath = filepath + "\\" + filename;
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await req.File.CopyToAsync(stream);
                }

                using (var _coreContext = new CoreContext())
                {
                    new_file.ExtensionType = extension;
                    new_file.Reference = filename;
                    new_file.Name = req.File.FileName;
                    new_file.Path = exactpath;
                    new_file.UpdatedDt = TxTimestamp;
                    new_file.CreatedDt = TxTimestamp;
                    _coreContext.FileTables.Add(new_file);
                    await _coreContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return new_file;
        }

        private string GenerateUniqueFileName()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string randomFileName = Path.GetRandomFileName().Replace(".", "");
            string uniqueFileName = $"{timestamp}_{randomFileName}";

            return uniqueFileName;
        }

        public async Task<FileTable> GetFileInfo(int fileId)
        {
            FileTable res = new FileTable();
            using (var _coreContext = new CoreContext())
            {
                res = _coreContext.FileTables.FirstOrDefault(x => x.FileId == fileId && x.IsDeleted == false);
                if (res == null) throw new Exception("File Not Found");
            }

            return res;
        }

        public async Task<DownloadFile_Response> DownloadFile(int fileId)
        {
            DownloadFile_Response res = new DownloadFile_Response();
            using (var _coreContext = new CoreContext())
            {
                FileTable? file = _coreContext.FileTables.FirstOrDefault(x => x.FileId == fileId && x.IsDeleted == false);
                if (file == null) throw new Exception("File Not Found");

                string filepath = file.Path;

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filepath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                res.ContentType = contentType;
                res.FileName = file.Name;
                res.Content = await System.IO.File.ReadAllBytesAsync(filepath);
                res.FilePath = filepath;
            }

            return res;
        }

        public async Task DeleteFile(int fileId)
        {
            try
            {
                using (var _coreContext = new CoreContext())
                {
                    FileTable? file = _coreContext.FileTables.FirstOrDefault(x => x.FileId == fileId && x.IsDeleted == false);
                    if (file == null) throw new Exception("File Not Found");

                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), file.Path);

                    // Check if file exists with its full path
                    if (File.Exists(filepath))
                    {
                        // If file found, delete it
                        File.Delete(Path.Combine(filepath));
                    }
                    else throw new Exception("File Not Found in " + filepath);

                    _coreContext.FileTables.Remove(file);
                    await _coreContext.SaveChangesAsync();
                }

            }
            catch (IOException ioExp)
            {
                throw new Exception(ioExp.Message);
            }

        }
    }
}
