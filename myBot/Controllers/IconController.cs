using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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
            var imageUrl = token.Users.Show(screen_name => screenName).ProfileImageUrl;
            switch (iconSize)
            {
                case IconSize.Mini:
                    imageUrl = new Uri(Regex.Replace(imageUrl.ToString(), @"_normal\.png$", "_mini.png", RegexOptions.IgnoreCase));
                    break;
                case IconSize.Large:
                    imageUrl = new Uri(Regex.Replace(imageUrl.ToString(), @"_normal\.png$", "_bigger.png", RegexOptions.IgnoreCase));
                    break;
                default:
                    break;
            }

            return new HttpClient().GetByteArrayAsync(imageUrl).Result;
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