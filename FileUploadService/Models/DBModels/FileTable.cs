using System;
using System.Collections.Generic;

namespace FileUploadService.Models.DBModels
{
    public partial class FileTable
    {
        public int FileId { get; set; }
        public string? Name { get; set; }
        public string? Reference { get; set; }
        public string? Path { get; set; }
        public string? ExtensionType { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public DateTime? CreatedDt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
