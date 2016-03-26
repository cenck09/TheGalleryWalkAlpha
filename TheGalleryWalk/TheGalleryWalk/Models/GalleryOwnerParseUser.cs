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
        public string Name;

        public string PhoneNumber;

        public int Enabled;


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