using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GeneralParseUser")]
    public class GeneralParseUser : ParseUser
    {
        [ParseFieldName("UserType")]
        public string UserType
            {
                get { return GetProperty<string>(); }
                set { SetProperty(value); }
            }


        [ParseFieldName("ImageURL")]
        public string ImageURL
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


    }
    
}