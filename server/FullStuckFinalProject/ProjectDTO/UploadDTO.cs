using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDTO
{
    public class UploadDTO
    {
        public string UserID { get; set; }
        public IFormFile file { get; set; }
    }
}
