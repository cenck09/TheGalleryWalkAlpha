using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;
using System.Diagnostics;


namespace TheGalleryWalk.Models
{
    [ParseClassName("GaleryOwnerParseUser")]
    public class GalleryOwnerParseUser : ParseUser
    {
        [ParseFieldName("Name")]
        public string Name
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

        [ParseFieldName("UserType")]
        public string UserType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public GalleryOwnerEntity toEntityWithSelf()
        {
            return new GalleryOwnerEntity()
            {
                Name = this.Name,
                EmailAddress = this.Email,
                PhoneNumber = this.PhoneNumber,
                ParseID = this.ObjectId,
                Enabled = this.Enabled
            };
        }

        public GalleryOwnerParseUser getInstanceFromParseObject(ParseUser user)
        {
            return new GalleryOwnerParseUser()
            {  
                Email = user.Email,
                Name = user.Get<string>("Name"),
                PhoneNumber = user.Get<string>("PhoneNumber"),
                Enabled = user.Get<int>("Enabled"),
                ObjectId = user.ObjectId
            };
        }

    }
}