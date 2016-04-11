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
    public class GalleryDirectoryController : BaseValidatorController
    {
        private ParseQuery<GalleryParseClass> getQuery()
        {
            return from item in new ParseQuery<GalleryParseClass>()
                       //where item.HasArtwork == 1
                   orderby item.Name ascending
                   select item;
        }
        public async Task<ActionResult> GalleryDirectory()
        {
            PageManager pageManager = (new PageManager()).setDefaultValues();
            await setPageManagerForQuery(pageManager, getQuery());

            pageManager.setPageMethodPost = "SetGalleryDirectoryPage";
            pageManager.incrementPageMethod = "IncrementGalleryDirectoryPage";
            pageManager.decrementPageMethod = "DecrementGalleryDirectoryPage";
            pageManager.pageMethodController = "GalleryDirectory";

            return await SetGalleryDirectoryPage(pageManager);
        }

        private ActionResult viewWithUserLayout(GalleryDirectoryManager directoryManager)
        {
            if (userIsGalleryOwner())
            { return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_LayoutLoggedIn", directoryManager); }
            else if (userIsArtist())
            { return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_LayoutArtistLoggedIn", directoryManager); }
            else
            { return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_Layout", directoryManager); }
        }
        private ParseQuery<GalleryParseClass> setQueryWithPageManager(ParseQuery<GalleryParseClass> query, PageManager pageManager)
        {
            return query.Skip((pageManager.currentPage - 1)).Limit(pageManager.pageItemCount);
        }
        private async Task setEntityArrayWithQuery(ParseQuery<GalleryParseClass> query, GalleryDirectoryManager directoryManager)
        {
            query = setQueryWithPageManager(query, directoryManager.PageManager);
            IList<GalleryEntity> galleries = new List<GalleryEntity>();

            foreach (GalleryParseClass g in await query.FindAsync())
            { galleries.Add(getGalleryEntity(g)); }

            directoryManager.galleryEntities = galleries;
        }

        public async Task<ActionResult> IncrementGalleryDirectoryPage(PageManager pageManager)
        {
            if (pageManager.currentPage < pageManager.totalPageCount)
            { pageManager.currentPage++; }
            return await SetGalleryDirectoryPage(pageManager);
        }
        public async Task<ActionResult> DecrementGalleryDirectoryPage(PageManager pageManager)
        {
            if (pageManager.currentPage > 1)
            { pageManager.currentPage--; }
            return await SetGalleryDirectoryPage(pageManager);
        }

        public async Task<ActionResult> SetGalleryDirectoryPage(PageManager pageManager)
        {
            GalleryDirectoryManager directoryManager = (new GalleryDirectoryManager()).setDefaults();
            directoryManager.PageManager = pageManager;

            await setEntityArrayWithQuery(getQuery(), directoryManager);
            return viewWithUserLayout(directoryManager);
        }
    }   
}