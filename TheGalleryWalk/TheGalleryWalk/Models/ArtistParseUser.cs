
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

        [ParseFieldName("UserType")]
        public string UserType
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


        public ArtistUserEntity toEntityWithSelf()
        {
            return new ArtistUserEntity()
            {
                Name = this.Name,
                EmailAddress = this.Email,
                PhoneNumber = this.PhoneNumber,
                ParseID = this.ObjectId,
                Enabled = this.Enabled
            };
        }

        public ArtistParseUser getInstanceFromParseObject(ParseUser user)
        {
            return new ArtistParseUser()
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