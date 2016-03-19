using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GaleryParseObject")]
    public class GalleryParseObject : ParseObject
    {
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        public string PhoneNumber
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }


    }
}