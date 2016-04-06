using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models { 

    [ParseClassName("ArtistParseClass")]
    public class ArtistParseClass: BaseParseObject
    {
      
       
        [ParseFieldName("Description")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("FileOwnerId")]
        public string FileOwnerId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Style")]
        public string Style
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Birth")]
        public string Birth
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


        [ParseFieldName("Death")]
        public string Death
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Image")]
        public Parse.ParseFile Image
        {
            get { return GetProperty<Parse.ParseFile>(); }
            set { SetProperty(value); }
        }


        /* This is so we can recreate the gallery entity so we can error check forms and 
              recreate the gallery object required for this view to render */
        [ParseFieldName("GalleryID")]
        public string GalleryID
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("GalleryOwnerID")]
        public string GalleryOwnerID
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}