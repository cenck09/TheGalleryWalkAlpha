
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;


namespace TheGalleryWalk.Models
{

    [ParseClassName("ArtworkParseClass")]
    public class ArtworkParseClass : ParseObject
    {
   
        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        [ParseFieldName("ArtistID")]
        public string ArtistID
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Description")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("GalleryID")]
        public string GalleryID
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}