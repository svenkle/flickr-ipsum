using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FlickrNet;
using Svenkle.ExtensionMethods;

namespace Website.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Index(int? width, int? height, string[] tag)
        {
            var flickr = new Flickr("", "");

            var searchResults = flickr.PhotosSearch(new PhotoSearchOptions
            {
                Extras = PhotoSearchExtras.LargeUrl | PhotoSearchExtras.OriginalDimensions,
                SafeSearch = SafetyLevel.Safe,
                Tags = string.Join(",", tag ?? Enumerable.Empty<string>()),
                TagMode = TagMode.AllTags
            }).ToList();

            IEnumerable<Photo> photos = searchResults;

            if (width.HasValue)
                photos = photos.Where(x => x.LargeWidth == width);

            if (height.HasValue)
                photos = photos.Where(x => x.LargeHeight == height);

            var photo = photos.Shuffle().FirstOrDefault();

            if (photo != null)
                return Redirect(photo.LargeUrl);

            return HttpNotFound("No image found with those requirements");
        }
    }
}