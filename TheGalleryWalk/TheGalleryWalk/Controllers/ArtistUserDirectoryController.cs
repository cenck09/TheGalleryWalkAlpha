using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGalleryWalk.Controllers
{
    public class ArtistUserDirectoryController : BaseValidatorController
    {
        private ParseQuery<GeneralParseUserData> getQuery()
        {
            return from item in new ParseQuery<GeneralParseUserData>()
                               where item.UserType == "ArtistUser"
                               where item.MyFavoriteGalleries != null
                               where item.MyFavoriteArtists != null
                               where item.Enabled == 1
                               where item.IsBanned == 0
                               where item.HasArtwork == 1
                               where item.AcceptedGalleryFollowers != null
                               select item;
        }
        private PageManager setPageManagerCallBack(PageManager pageManager)
        {
            pageManager.setPageMethodPost = "PostArtistUserDirectoryPage";
            pageManager.incrementPageMethod = "IncrementArtistUserDirectoryPage";
            pageManager.decrementPageMethod = "DecrementArtistUserDirectoryPage";
            pageManager.pageMethodController = "ArtistUserDirectory";
            pageManager.setPageMethod = "SetArtistUserDirectoryPage";
            return pageManager;
        }
        private ActionResult viewWithUserLayout(ArtistUserDirectoryManager directoryManager)
        {
            Debug.WriteLine("enter viewWithUserLayout");
            if (userIsGalleryOwner())
            {
                return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutLoggedIn", directoryManager);
            }
            else if (userIsArtist())
            {
                return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutArtistLoggedIn", directoryManager);
            }
            else
            {
                return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_Layout", directoryManager);
            }

        }
        private ParseQuery<GeneralParseUserData> setQueryWithPageManager(ParseQuery<GeneralParseUserData> query, PageManager pageManager)
        {
            return query.Skip(((pageManager.currentPage - 1)) * pageManager.pageItemCount).Limit(pageManager.pageItemCount);
        }
        private async Task setEntityArrayWithQuery(ParseQuery<GeneralParseUserData> query, ArtistUserDirectoryManager directoryManager)
        {
            query = setQueryWithPageManager(query, directoryManager.PageManager);
            IList<ArtistUserEntity> artists = new List<ArtistUserEntity>();

            foreach (GeneralParseUserData g in await query.FindAsync())
            { artists.Add(getArtistUserEntity(g)); }

            directoryManager.artistUserEntities = artists;
        }

        public async Task<ActionResult> ArtistUserDirectory()
        {
            PageManager manager = (new PageManager()).setDefaultValues();
            manager = await setPageManagerForQuery(manager, getQuery());
            manager = setPageManagerCallBack(manager);
            return await SetArtistUserDirectoryPage(manager);
        }
        public async Task<ActionResult> IncrementArtistUserDirectoryPage(PageManager pageManager)
        {
            if (pageManager.currentPage < pageManager.totalPageCount)
            { pageManager.currentPage++; }
            return await SetArtistUserDirectoryPage(pageManager);
        }
        public async Task<ActionResult> DecrementArtistUserDirectoryPage(PageManager pageManager)
        {
            if (pageManager.currentPage > 1)
            { pageManager.currentPage--; }
            return await SetArtistUserDirectoryPage(pageManager);
        }

        public async Task<ActionResult> SetArtistUserDirectoryPage(PageManager pageManager)
        {
            ArtistUserDirectoryManager directoryManager = (new ArtistUserDirectoryManager()).setDefaults();

            directoryManager.PageManager.currentPage = pageManager.currentPage;
            directoryManager.PageManager.totalPageCount = pageManager.totalPageCount;
            directoryManager.PageManager.totalItemCount = pageManager.totalItemCount;

            directoryManager.PageManager = setPageManagerList(directoryManager.PageManager);
            directoryManager.PageManager = setPageManagerCallBack(directoryManager.PageManager);

            await setEntityArrayWithQuery(getQuery(), directoryManager);

            return viewWithUserLayout(directoryManager);
        }

        [HttpPost]
        public async Task<ActionResult> PostArtistUserDirectoryPage(PageManager pageManager)
        { return await SetArtistUserDirectoryPage(pageManager); }

    /*    public async Task<ActionResult> ArtistUserDirectory()
        {
            // This query will be updated to remove artists without artwork and other things we'll need
            var query = from item in new ParseQuery<GeneralParseUserData>()
                        where item.UserType == "ArtistUser"
                        where item.MyFavoriteGalleries != null
                        where item.MyFavoriteArtists != null
                        where item.Enabled == 1
                        where item.IsBanned == 0
                        where item.HasArtwork == 1
                        where item.AcceptedGalleryFollowers != null
                        select item;

            IList<ArtistUserEntity> artists = new List<ArtistUserEntity>();

            foreach (GeneralParseUserData artistuser in await query.FindAsync())
            {
                artists.Add(getArtistUserEntity(artistuser));
            }

           if (userIsGalleryOwner())
           {
              return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutLoggedIn", artists);
           }
           else if(userIsArtist())
           {
             return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutArtistLoggedIn", artists);
           }
           else
           {
             return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_Layout", artists);
           }
        }*/
    }
}