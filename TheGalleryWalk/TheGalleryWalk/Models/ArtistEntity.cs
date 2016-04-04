
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
    public class ArtistEntity : BaseEntity
    {
       
        [DisplayName("Style")]
        public string Style { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Birth")]
        public string Birth { get; set; }

        [DisplayName("Death")]
        public string Death { get; set; }

       [DisplayName("ImageFile")]
       public Parse.ParseFile Image { get; set; }

        public IList<ArtworkEntity> ArtworkEntities { get; set; }
        public string ParentGalleryParseID { get; set; }

    }
}