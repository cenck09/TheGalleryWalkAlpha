using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGalleryWalk.Models
{
    public class GalleryDirectoryManager : BaseEntity
    {
        public IEnumerable<GalleryEntity> galleryEntities { get; set; }
        public GalleryDirectoryManager setDefaults()
        {
            galleryEntities = new List<GalleryEntity>();
            PageManager = (new PageManager()).setDefaultValues();
            return this;
        }
    }
}