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
    public class ArtworkEntity : BaseEntity
    {

        [Required(ErrorMessage = "Please enter an Artist."),
             StringLength(30, MinimumLength = 2, ErrorMessage = "Please enter an Artist with more than one letter.")]
        [DisplayName("Artist")]
        public string Artist { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Style")]
        public string Style { get; set; }


        public string ParentGalleryParseID { get; set; }
        public List<SelectListItem> addArtworkFormlistItem { get; set; }
        public List<SelectListItem> GalleryListForArtworkSharing { get; set; }
        public string OwnershipState { get; set; }
        public bool ShouldAddSharingOptions { get; set; }
    }
}