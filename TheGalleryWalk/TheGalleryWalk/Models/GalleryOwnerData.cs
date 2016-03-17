using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Models
{
    public class GalleryOwnerData 
    {
       
      [Key]
      public int ID { get; set; }
      public string Name { get; set; }
      public string PhoneNumber { get; set; }
      public string EmailAddress { get; set; }

      public GalleryEntity[] galleries { get; set; }

    }
}