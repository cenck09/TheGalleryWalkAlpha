using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;


namespace TheGalleryWalk.Models
{
    public class ArtworkEntity
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a name."),
              StringLength(30, MinimumLength = 2, ErrorMessage = "Please enter a name with more than one letter.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an Artist."),
             StringLength(30, MinimumLength = 2, ErrorMessage = "Please enter a name with more than one letter.")]
        [DisplayName("Artist")]
        public string Artist { get; set; }

        public string Description { get; set; }


        public IList<string> Artworks { get; set; }
        public IEnumerable<ParseObject> ArtworkEntities { get; set; }
        public GalleryEntity ArtworkAdd { get; set; }
        public string ParentGalleryParseID { get; set; }

        public List<SelectListItem> addArtistFormlistItem;
    }
}