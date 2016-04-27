using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string parseId { get; set; }
        public string incrementPageMethod { get; set; }
        public string decrementPageMethod { get; set; }
        public string setPageMethodPost { get; set; }
        public string setPageMethod { get; set; }
        public string pageMethodController { get; set; }
        public List<SelectListItem> pageNumberList { get; set; }

        public PageManager setDefaultValues()
        {
            currentPage = totalPageCount = totalItemCount = 1;
            pageItemCount = 2;
            incrementPageMethod = decrementPageMethod = setPageMethod = pageMethodController  = parseId = setPageMethodPost =  "";
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
            setPageMethod = manager.setPageMethod;
            pageNumberList = manager.pageNumberList;
            parseId = manager.parseId;
            return this;
        }

        public void printPageManagerContents(PageManager manager)
        {
            Debug.WriteLine("\n------- Manager Total page : " + manager.totalPageCount);
            Debug.WriteLine("\n------- Manager Total Item count : " + manager.totalItemCount);
            Debug.WriteLine("\n------- Manager number list count : " + manager.pageNumberList.Count);
            Debug.WriteLine("\n------- Manager tracked Parse ID  : " + manager.parseId);
        }
    }
}