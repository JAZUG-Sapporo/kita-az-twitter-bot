using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CoreTweet;
using Microsoft.Owin.Security.Twitter;
using Newtonsoft.Json;
using Toolbelt.Web;

namespace myBot.Controllers
{
    [Authorize]
    public class IconController : Controller
    {
        private enum IconSize
        {
            Normal,
            Mini,
            Large
        }

        private byte[] GetProfileImage(string screenName, IconSize iconSize)
        {
            var twitterAuthOpt = JsonConvert.DeserializeObject<TwitterAuthenticationOptions>(AppSettings.Key.Twitter);
            var token = CoreTweet.OAuth2.GetToken(twitterAuthOpt.ConsumerKey, twitterAuthOpt.ConsumerSecret);
            try
            {
                var imageUrl = token.Users.Show(screen_name => screenName).ProfileImageUrl;
                switch (iconSize)
                {
                    case IconSize.Mini:
                        imageUrl = Regex.Replace(imageUrl.ToString(), @"_normal\.png$", "_mini.png", RegexOptions.IgnoreCase);
                        break;
                    case IconSize.Large:
                        imageUrl = Regex.Replace(imageUrl.ToString(), @"_normal\.png$", "_bigger.png", RegexOptions.IgnoreCase);
                        break;
                    default:
                        break;
                }
                return new HttpClient().GetByteArrayAsync(imageUrl).Result;
            }
            catch (TwitterException e)
            {
                if (e.Status != HttpStatusCode.NotFound && e.Status != HttpStatusCode.Forbidden) throw;
                return System.IO.File.ReadAllBytes(this.Server.MapPath("~/Content/images/no-image.png"));
            }
        }

        public ActionResult Normal(string id)
        {
            return new CacheableContentResult(
                contentType: "image/png",
                cacheability: HttpCacheability.ServerAndPrivate,
                etag: id,
                getContent: () => GetProfileImage(id, IconSize.Normal)
            );
        }

        public ActionResult Mini(string id)
        {
            return new CacheableContentResult(
                contentType: "image/png",
                cacheability: HttpCacheability.ServerAndPrivate,
                etag: id,
                getContent: () => GetProfileImage(id, IconSize.Mini)
            );
        }

        public ActionResult Large(string id)
        {
            return new CacheableContentResult(
                contentType: "image/png",
                cacheability: HttpCacheability.ServerAndPrivate,
                etag: id,
                getContent: () => GetProfileImage(id, IconSize.Large)
            );
        }
    }
}