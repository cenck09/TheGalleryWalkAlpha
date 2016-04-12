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
    public class GalleryInformationController : BaseValidatorController
    {
        public async Task<ActionResult> GalleryInformationView(GalleryEntity gallery)
        {
            PageManager pageManager = (new PageManager()).setDefaultValues();
            pageManager.parseId = gallery.ParseID;
            return await SetGalleryInformationPage(pageManager);
        }

        public async Task<ActionResult> IncrementGalleryInformationPage(PageManager pageManager)
        {
            if (pageManager.currentPage < pageManager.totalPageCount)
            { pageManager.currentPage++; }
            return await SetGalleryInformationPage(pageManager);
        }
        public async Task<ActionResult> DecrementGalleryInformationPage(PageManager pageManager)
        {
            if (pageManager.currentPage > 1)
            { pageManager.currentPage--; }
            return await SetGalleryInformationPage(pageManager);
        }
        public async Task<ActionResult> SetGalleryInformationPage(PageManager pageManager)
        {
            GalleryEntity gallery = await getGalleryEntityWithID(pageManager.parseId);
            gallery.PageManager = (new PageManager()).setDefaultValues();
            gallery.PageManager.parseId = pageManager.parseId;

            gallery.PageManager.currentPage = pageManager.currentPage;
            gallery.PageManager.totalPageCount = pageManager.totalPageCount;
            gallery.PageManager.totalItemCount = pageManager.totalItemCount;
            gallery.PageManager = await setPageManagerForQuery(gallery.PageManager, getArtworkQueryForGalleryID(pageManager.parseId));

            gallery.PageManager = setPageManagerList(gallery.PageManager);
            gallery.PageManager = setPageManagerCallBack(gallery.PageManager);

            await setEntityArrayWithQuery(getArtworkQueryForGalleryID(gallery.ParseID), gallery);

            return viewWithUserLayout(gallery);
        }

        [HttpPost]
        public async Task<ActionResult> PostGalleryInformationPage(PageManager pageManager)
        { return await SetGalleryInformationPage(pageManager); }

        private ParseQuery<ArtworkParseClass> getArtworkQueryForGalleryID(string galleryID)
        {
            return from item in new ParseQuery<ArtworkParseClass>()
                   where item.GalleryID == galleryID
                   orderby item.Name ascending
                   select item;
        }
        private async Task<GalleryEntity> getGalleryEntityWithID(string parseID)
        {
            return getGalleryEntity( await (from item in new ParseQuery<GalleryParseClass>()
                    where item.ObjectId == parseID
                   select item).FirstAsync());
        }
        private PageManager setPageManagerCallBack(PageManager pageManager)
        {
            pageManager.setPageMethodPost = "PostGalleryInformationPage";
            pageManager.incrementPageMethod = "IncrementGalleryInformationPage";
            pageManager.decrementPageMethod = "DecrementGalleryInformationPage";
            pageManager.pageMethodController = "GalleryInformation";
            pageManager.setPageMethod = "SetGalleryInformationPage";
            return pageManager;
        }
        private ActionResult viewWithUserLayout(GalleryEntity gallery)
        {
            if (userIsGalleryOwner())
            { return View("~/Views/GalleryInformation/GalleryInformationView.cshtml", "_LayoutLoggedIn", gallery); }
            else if (userIsArtist())
            { return View("~/Views/GalleryInformation/GalleryInformationView.cshtml", "_LayoutArtistLoggedIn", gallery); }
            else
            { return View("~/Views/GalleryInformation/GalleryInformationView.cshtml", "_Layout", gallery); }
        }
        private ParseQuery<ArtworkParseClass> setQueryWithPageManager(ParseQuery<ArtworkParseClass> query, PageManager pageManager)
        {
            return query.Skip(((pageManager.currentPage - 1)) * pageManager.pageItemCount).Limit(pageManager.pageItemCount);
        }
        private async Task setEntityArrayWithQuery(ParseQuery<ArtworkParseClass> query, GalleryEntity gallery)
        {
            query = setQueryWithPageManager(query, gallery.PageManager);
            Debug.WriteLine("Number of results = ");
            IList<ArtworkEntity> artworks = new List<ArtworkEntity>();
            Debug.WriteLine("");
            foreach (ArtworkParseClass g in await query.FindAsync())
            { artworks.Add(getArtworkEntity(g)); }

            gallery.ArtworkEntities = artworks;
        }
    }
}