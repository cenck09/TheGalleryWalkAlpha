using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public PageManager setDefaultValues()
        {
            currentPage = totalPageCount = totalItemCount = 0;
            pageItemCount = 20;
            incrementPageMethod = decrementPageMethod = "";
            return this;
        }
    }
}