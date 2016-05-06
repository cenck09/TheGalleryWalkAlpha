using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.IO;

namespace TheGalleryWalk.Controllers
{
    public class Artist_Async_SignUpController : BaseValidatorController
    {
        public ActionResult Signup()
        {
            return View("~/Views/Artist_Async_SignUp/Signup.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Signup(ArtistUserEntity registerData, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {


                byte[] data;

                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                var name = "photo.jpg";
                var parseFile = new Parse.ParseFile(name, data);

                try
                {

                    if (parseFile.IsDirty)
                    {
                        await parseFile.SaveAsync();
                        Debug.WriteLine("Data to save");

                    }
                    else
                    {
                        Debug.WriteLine("No data to save");
                    }

                    Debug.WriteLine(parseFile.Url.ToString());


                    Debug.WriteLine("IMAGE SAVED SUCCESSFULLY");
                }
                catch
                {
                    Debug.WriteLine("IMAGE COULD NOT BE SAVED");
                }

                var fileUrl = parseFile.Url.ToString();


                var user = new GeneralParseUser()
                {
                    Username = registerData.EmailAddress,
                    Password = registerData.Password,
                    Email = registerData.EmailAddress,
                    UserType = "ArtistUser"
                };

             
                try { await user.SignUpAsync(); }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignUp");
                }

                var userInfo = new GeneralParseUserData()
                {
                    Name = registerData.Name,
                    PhoneNumber = registerData.PhoneNumber,
                    Enabled = 1, // enabled by default, will set to 0 before production 
                    UserType = "ArtistUser",
                    UserId = user.ObjectId,
                    MyFavoriteGalleries = new List<string>(),
                    MyFavoriteArtists = new List<string>(),
                    HasArtwork = 0,
                    IsBanned = 1,
                    ImageURL = fileUrl,
                    Email = user.Email,
                    AcceptedGalleryFollowers = new List<string>(),
                };

                try
                {
                    await userInfo.SaveAsync();
                    return View("CompletedSignUp");
                }
                catch (Exception ex)
                {
                    await user.DeleteAsync();
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignUp");
                }

            }// end if ModelState.IsValid

            return View();
        }
    }
}