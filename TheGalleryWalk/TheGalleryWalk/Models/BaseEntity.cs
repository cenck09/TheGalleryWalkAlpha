using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheGalleryWalk.Models
{
    public class BaseEntity
    {

        public string ParseID { get; set; }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a name."),
              StringLength(30, MinimumLength = 2, ErrorMessage = "Please enter a name with more than one letter.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("ImageURL")]
        public string ImageURL { get; set; }
    }
}