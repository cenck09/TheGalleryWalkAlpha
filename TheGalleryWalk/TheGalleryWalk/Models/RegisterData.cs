using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Models
{

 

    public class RegisterData
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a name."),
        StringLength(30, MinimumLength = 2, ErrorMessage = "Please enter a name with more than one letter.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a Password."),
        StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter a valid Password between 6 and 20 digits.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Passwords do not match"),
         System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match."),
          StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter a valid Password between 6 and 20 digits.")]
        [DisplayName("Reenter your Password")]
        public string PasswordCheck { get; set; }


        [Required(ErrorMessage = "Please enter your Phone Number.")]
        [DisplayName("Phone Number")]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your email address."),
        StringLength(50, ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [StringLength(200, ErrorMessage = "Please enter a valid address.")]
        [DisplayName("Address")]
        public string Address { get; set; }

    }


}

