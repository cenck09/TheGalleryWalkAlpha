using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Models
{
    public class PageManager
    {
        public int currentPage { get; set; }
        public int pageItemCount { get; set; }
        public int totalPageCount { get; set; }
        public int totalItemCount { get; set; }

        public string incrementPageMethod { get; set; }
        public string decrementPageMethod { get; set; }
        public string setPageMethodPost { get; set; }
        public string pageMethodController { get; set; }
        public List<SelectListItem> pageNumberList { get; set; }

        public PageManager setDefaultValues()
        {
            currentPage = totalPageCount = totalItemCount = 1;
            pageItemCount = 4;
            incrementPageMethod = decrementPageMethod = pageMethodController = setPageMethodPost =  "";
            pageNumberList = new List<SelectListItem>();
            return this;
        }
        public PageManager copyPageManager(PageManager manager)
        {
            currentPage = manager.currentPage;
            totalPageCount = manager.totalPageCount;
            totalItemCount = manager.totalItemCount;
            pageItemCount = manager.pageItemCount;
            incrementPageMethod = manager.incrementPageMethod;
            decrementPageMethod = manager.decrementPageMethod;
            pageMethodController = manager.pageMethodController;
            setPageMethodPost = manager.setPageMethodPost;
            pageNumberList = manager.pageNumberList;
            return this;
        }
    }
}