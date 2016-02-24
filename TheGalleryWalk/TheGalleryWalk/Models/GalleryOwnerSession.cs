using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Models
{
    public class GalleryOwnerSession 
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SessionID { get; set; }
    }
}