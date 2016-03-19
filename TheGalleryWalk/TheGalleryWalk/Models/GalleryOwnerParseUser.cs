using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GaleryOwnerParseUser")]
    public class GalleryOwnerParseUser : ParseUser
    {
        public string Name;

        public string PhoneNumber;

        public Boolean Enabled;

        public IList<string> Galleries;

    }
}