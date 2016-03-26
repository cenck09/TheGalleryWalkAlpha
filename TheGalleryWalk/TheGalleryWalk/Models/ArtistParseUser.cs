
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("ArtistParseUser")]
    public class ArtistParseUser : ParseUser
    {   
        public string Name;

        public string PhoneNumber;

        public Boolean Enabled;

        public IList<string> Artwork;

     
    }
}