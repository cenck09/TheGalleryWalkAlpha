using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGalleryWalk.Controllers
{
    public class ArtistUserInformationController : BaseValidatorController
    {
        public async Task<ActionResult> ArtistUserInformationView(ArtistUserEntity artistUser)
        {
            PageManager pageManager = (new PageManager()).setDefaultValues();
            pageManager.parseId = artistUser.ParseID;
            return await SetArtistUserInformationPage(pageManager);
        }

        public async Task<ActionResult> IncrementArtistUserInformationPage(PageManager pageManager)
        {
            if (pageManager.currentPage < pageManager.totalPageCount)
            { pageManager.currentPage++; }
            return await SetArtistUserInformationPage(pageManager);
        }
        public async Task<ActionResult> DecrementArtistUserInformationPage(PageManager pageManager)
        {
            if (pageManager.currentPage > 1)
            { pageManager.currentPage--; }
            return await SetArtistUserInformationPage(pageManager);
        }
        public async Task<ActionResult> SetArtistUserInformationPage(PageManager pageManager)
        {
            ArtistUserEntity artistUser = await getArtistUserEntityWithID(pageManager.parseId);
            artistUser.PageManager = (new PageManager()).setDefaultValues();
            artistUser.PageManager.parseId = pageManager.parseId;

            artistUser.PageManager.currentPage = pageManager.currentPage;
            artistUser.PageManager.totalPageCount = pageManager.totalPageCount;
            artistUser.PageManager.totalItemCount = pageManager.totalItemCount;
            artistUser.PageManager = await setPageManagerForQuery(artistUser.PageManager, getArtworkQueryForArtistUserID(pageManager.parseId));

            artistUser.PageManager = setPageManagerList(artistUser.PageManager);
            artistUser.PageManager = setPageManagerCallBack(artistUser.PageManager);

            await setEntityArrayWithQuery(getArtworkQueryForArtistUserID(artistUser.ParseID), artistUser);

            return viewWithUserLayout(artistUser);
        }

        [HttpPost]
        public async Task<ActionResult> PostArtistUserInformationPage(PageManager pageManager)
        { return await SetArtistUserInformationPage(pageManager); }

        private ParseQuery<ArtworkParseClass> getArtworkQueryForArtistUserID(string userID)
        {
            return from item in new ParseQuery<ArtworkParseClass>()
                   where item.ArtistID == userID
                   orderby item.Name ascending
                   select item;
        }
        private async Task<ArtistUserEntity> getArtistUserEntityWithID(string parseID)
        {
            return getArtistUserEntity( await (from item in new ParseQuery<GeneralParseUserData>()
                                               where item.UserType == "ArtistUser"
                                               where item.MyFavoriteGalleries != null
                                               where item.MyFavoriteArtists != null
                                               where item.Enabled == 1
                                               where item.IsBanned == 0
                                               where item.HasArtwork == 1
                                               where item.AcceptedGalleryFollowers != null
                                               where item.ObjectId == parseID
                   select item).FirstAsync());
        }
        private PageManager setPageManagerCallBack(PageManager pageManager)
        {
            pageManager.setPageMethodPost = "PostArtistUserInformationPage";
            pageManager.incrementPageMethod = "IncrementArtistUserInformationPage";
            pageManager.decrementPageMethod = "DecrementArtistUserInformationPage";
            pageManager.pageMethodController = "ArtistUserInformation";
            pageManager.setPageMethod = "SetArtistUserInformationPage";
            return pageManager;
        }
        private ActionResult viewWithUserLayout(ArtistUserEntity artistUser)
        {
            if (userIsGalleryOwner())
            { return View("~/Views/ArtistUserInformation/ArtistUserInformationView.cshtml", "_LayoutLoggedIn", artistUser); }
            else if (userIsArtist())
            { return View("~/Views/ArtistUserInformation/ArtistUserInformationView.cshtml", "_LayoutArtistLoggedIn", artistUser); }
            else
            { return View("~/Views/ArtistUserInformation/ArtistUserInformationView.cshtml", "_Layout", artistUser); }
        }
        private ParseQuery<ArtworkParseClass> setQueryWithPageManager(ParseQuery<ArtworkParseClass> query, PageManager pageManager)
        {
            return query.Skip(((pageManager.currentPage - 1)) * pageManager.pageItemCount).Limit(pageManager.pageItemCount);
        }
        private async Task setEntityArrayWithQuery(ParseQuery<ArtworkParseClass> query, ArtistUserEntity artistUser)
        {
            query = setQueryWithPageManager(query, artistUser.PageManager);
            Debug.WriteLine("Number of results = ");
            IList<ArtworkEntity> artworks = new List<ArtworkEntity>();
            Debug.WriteLine("");
            foreach (ArtworkParseClass g in await query.FindAsync())
            { artworks.Add(getArtworkEntity(g)); }

            artistUser.ArtworkEntities = artworks;
        }
    }
}