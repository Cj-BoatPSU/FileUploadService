namespace FileUploadService.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public int? currentpage { get; set; }
        public int? totalrecord { get; set; }
        public object? data { get; set; }
        public ExceptionModel? errors { get; set; }
    }

    public class ExceptionModel
    {
        public string message { get; set; }
    }
}
