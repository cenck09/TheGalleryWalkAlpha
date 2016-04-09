using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    public class BaseParseObject : ParseObject
    {

        [ParseFieldName("Name")]
        public string Name
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