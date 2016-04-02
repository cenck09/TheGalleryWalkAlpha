using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GeneralParseUserData")]
    public class GeneralParseUserData : BaseParseObject
    {
        [ParseFieldName("UserId")]
        public string UserId
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
            get { return GetProperty<IList<string>>(); }
            set { SetProperty(value); }
        }


        [ParseFieldName("MyFavoriteArtists")]
        public IList<string> MyFavoriteArtists
        {
            get { return GetProperty<IList<string>>(); }
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