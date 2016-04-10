using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GalleryParseClass")]
    public class GalleryParseClass : BaseParseObject
    {
      
        [ParseFieldName("PhoneNumber")]
        public string PhoneNumber
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Address")]
        public string Address
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

        [ParseFieldName("Email")]
        public string Email
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Website")]
        public string Website
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

        [ParseFieldName("IsBanned")]
        public int IsBanned
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("HasArtwork")]
        public int HasArtwork
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }
    }
}