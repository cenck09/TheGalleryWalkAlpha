using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGalleryWalk.Models
{
    public class ArtistUserDirectoryManager : BaseEntity
    {
        public IEnumerable<ArtistUserEntity> artistUserEntities { get; set; }
        public ArtistUserDirectoryManager setDefaults()
        {
            artistUserEntities = new List<ArtistUserEntity>();
            PageManager = (new PageManager()).setDefaultValues();
            return this;
        }
    }
}