using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    public class GeneralParseUserData : ParseObject
    {
        [ParseFieldName("UserId")]
        public string UserId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        [ParseFieldName("UserType")]
        public string UserType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("PhoneNumber")]
        public string PhoneNumber
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


        [ParseFieldName("Enabled")]
        public int Enabled
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }


        [ParseFieldName("MyFavoriteGalleries")]
        public IList<string> MyFavoriteGalleries
        {
            get { return GetProperty<List<string>>(); }
            set { SetProperty(value); }
        }


        [ParseFieldName("MyFavoriteArtists")]
        public IList<string> MyFavoriteArtists
        {
            get { return GetProperty<List<string>>(); }
            set { SetProperty(value); }
        }



    }
}