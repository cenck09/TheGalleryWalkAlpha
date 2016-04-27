

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;

namespace TheGalleryWalk.Models
{
    public class ArtistUserEntity : BaseEntity
    {
       
        [Required(ErrorMessage = "Please enter a Password."),
        StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter a valid Password between 6 and 20 digits.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Passwords do not match"),
         System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match."),
        StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter a valid Password between 6 and 20 digits.")]
        [DisplayName("Confirm Password")]
        public string PasswordCheck { get; set; }


        [Required(ErrorMessage = "Please enter your Phone Number."),
        RegularExpression("^[01]?[- .]?(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[- .]?\\d{3}[- .]?\\d{4}$", ErrorMessage = "Please enter the number with the area code ###-###-####")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your email address."),
         RegularExpression("^(?(\"\")(\"\".+?\"\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$", ErrorMessage = "Please enter a valid Email Address"),
        StringLength(100, ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        public int Enabled { get; set; }
        public ArtworkEntity ArtworkAdd { get; set; }
        public IList<ArtworkEntity> ArtworkEntities { get; set; }
        public IList<string> PermittedGalleries { get; set; }
        public IList<GalleryOwnerEntity> OwnersFollowingThisArtistUser { get; set; }
    }
}

