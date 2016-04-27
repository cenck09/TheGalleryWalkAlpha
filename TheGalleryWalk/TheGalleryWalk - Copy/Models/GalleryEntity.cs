﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;

namespace TheGalleryWalk.Models
{
    public class GalleryEntity : BaseEntity
    {

        [Required(ErrorMessage = "Please enter your Phone Number."),
               RegularExpression("^[01]?[- .]?(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[- .]?\\d{3}[- .]?\\d{4}$", ErrorMessage = "Please enter the number with the area code ###-###-####")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your email address."),
         RegularExpression("^(?(\"\")(\"\".+?\"\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$", ErrorMessage = "Please enter a valid Email Address"),
        StringLength(100, ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [StringLength(200, ErrorMessage = "Please enter a valid address.")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [StringLength(200, ErrorMessage = "Please enter a valid website.")]
        [DisplayName("Website")]
        public string Website { get; set; }

        public string GalleryOwnerID { get; set; }

        public IList<ArtworkEntity> ArtworkEntities { get; set; }
        public ArtworkEntity ArtworkAdd { get; set; }

        public IList<ArtistEntity> ArtistEntities { get; set; }
        public IList<ArtistUserEntity> ArtistUserEntities { get; set; }
        public ArtistEntity ArtistAdd { get; set; }

    }

}