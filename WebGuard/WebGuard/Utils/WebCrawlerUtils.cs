using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CefSharp;

namespace WebGuard.Utils
{
    public static class WebCrawlerUtil
    {
        public static async Task<IList<string>> GetAllPageUrl(this IWebBrowser brw)
        {
            var lstLevel = new List<IList<string>> { await brw.GetPageUrls() };
            var urlHashSet = new HashSet<string>(lstLevel[0]);
            var curUrls = null as IList<string>;
            var isBrwDoneLoading = false;

            brw.LoadingStateChanged += BrwOnLoadingStateChanged;

            for (var i = 0; i < lstLevel.Count; i++)
            {
                foreach (var url in lstLevel[i])
                {
                    curUrls = null;
                    isBrwDoneLoading = false;
                    brw.Load(url);

                    //Wait for curUrls to be initialized
                    while (curUrls == null)
                    {
                        await Task.Delay(1);
                    }

                    foreach (var curUrl in curUrls)
                    {
                        urlHashSet.Add(curUrl);
                    }

                    lstLevel.Add(urlHashSet.ToArray());
                }
            }

            return lstLevel[lstLevel.Count - 1];

            //Local
            async void BrwOnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
            {
                // ReSharper disable once AccessToModifiedClosure
                if (!e.IsLoading || isBrwDoneLoading) return;
                isBrwDoneLoading = true;

                curUrls = await brw.GetPageUrls();
            }
        }

        /// <summary>
        /// Get all url (href of a tag) in a single page
        /// </summary>
        /// <param name="brw"></param>
        /// <returns></returns>
        public static async Task<IList<string>> GetPageUrls(this IWebBrowser brw)
        {
            var urlHashSet = new HashSet<string>();
            var result = (await brw.EvaluateScriptAsync(
                "Array.prototype.forEach.call(document.getElementsByTagName(\"a\"), (ele) => {console.log(ele.href)})")).Result.ToString().Split('\n');

            //Remove same URL in the result by a hash set
            foreach (var ele in result)
            {
                urlHashSet.Add(ele);
                await Task.Delay(1);
            }

            return urlHashSet.ToArray();
        }
    }
}
