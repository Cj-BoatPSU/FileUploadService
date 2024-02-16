using System;
using System.Collections.Generic;

namespace FileUploadService.Models.DBModels
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public DateTime? CreatedDt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
