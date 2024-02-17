namespace FileUploadService.Models.ServiceModels
{
    public class FileModel
    {
        public class UploadFile_Req
        {
            public IFormFile? File { get; set; }
            public int? AccountId { get; set; }
        }

        public class DownloadFile_Req
        {
            public int? FileId { get; set; }
        }

        public class DeleteFile_Req
        {
            public int? FileId { get; set; }
        }

        public class DownloadFile_Response
        {
            public byte[] Content { get; set; }
            public string ContentType { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
        }
    }
}
