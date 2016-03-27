﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parse;

namespace TheGalleryWalk.Models
{
    [ParseClassName("GalleryParseClass")]
    public class GalleryParseClass : ParseObject
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

        [ParseFieldName("Address")]
        public string Address
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


        public GalleryEntity toEntityWithSelf()
        {
            return new GalleryEntity()
            {
                Name = this.Name,
                EmailAddress = this.Email,
                PhoneNumber = this.PhoneNumber,
                ParseID = this.ObjectId,
                GalleryOwnerID = this.GalleryOwnerID,
                Website = this.Website,
                Address = this.Address
            };
        }
    }
}