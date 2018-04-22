using System;
using CefSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGuard.CustomBrowser;

namespace WebGuard.Utils
{
    public static class WebCrawlerUtil
    {
        public static async Task<IList<string>> GetAllPageUrlWithSameOrigin(this ChromiumWithScript brw)
        {
            var lstLevel = new List<IList<string>> { await brw.GetPageUrlsWithSameOrigin() };
            var urlHashSet = new HashSet<string>(lstLevel[0]);
            var alreadyGetUrlHashSet = new HashSet<string>();
            var curUrls = null as IList<string>;
            var isBrwDoneLoading = false;

            brw.LoadingStateChanged += BrwOnLoadingStateChanged;

            for (var i = 0; i < lstLevel.Count; i++)
            {
                foreach (var url in lstLevel[i])
                {
                    //If we already crawling the url, skip it
                    if (alreadyGetUrlHashSet.Contains(url)) continue;

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

                    alreadyGetUrlHashSet.Add(url);
                }
            }

            return lstLevel[lstLevel.Count - 1];

            //Local
            async void BrwOnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
            {
                // ReSharper disable once AccessToModifiedClosure
                if (e.IsLoading || isBrwDoneLoading) return;
                isBrwDoneLoading = true;

                curUrls = await brw.GetPageUrlsWithSameOrigin();
            }
        }

        /// <summary>
        /// Get all url (href of a tag) in a single page
        /// </summary>
        /// <param name="brw"></param>
        /// <returns></returns>
        public static async Task<IList<string>> GetPageUrlsWithSameOrigin(this ChromiumWithScript brw)
        {
            var urlHashSet = new HashSet<string>();
            await brw.EvaluateScriptAsync("var urls = \"\"");
            await brw.EvaluateScriptAsync(
                "Array.prototype.forEach.call(document.getElementsByTagName(\"a\"), (ele) => {urls += ele.href + \",\"})");
            await brw.EvaluateScriptAsync("console.log(urls)");
            var urls = brw.ConsoleOutput;

            //Filter null/empty href and different origins
             var result = urls.Split(',')
                 .Where(x => !string.IsNullOrEmpty(x) && x.IndexOf(brw.Origin, StringComparison.OrdinalIgnoreCase) != -1);

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
