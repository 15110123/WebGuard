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

        public static async Task<string> findget(this ChromiumWithScript brw, String url)
        {
            bool isloaded= await loadurl(brw,url);
            int soform = -1;
            var phuongthuc = "";
            String script = "<Script> console.log(\"Hello World\") </Script>";
            String kq = "";
            
            //Tìm số form 
            await brw.EvaluateScriptAsync("var soform");
            await brw.EvaluateScriptAsync(
                    "soform = document.forms.length");
            await brw.EvaluateScriptAsync("console.log(soform)");
            soform = Int32.Parse(brw.ConsoleOutput);

            if (soform > 0)
                for (int i = 0; i < soform; i++)
                {
                    //Tìm phương thức của từng form
                    await brw.EvaluateScriptAsync("var phuongthuc");
                    await brw.EvaluateScriptAsync(
                            "phuongthuc = document.forms[" + i + "].getAttribute('method')");
                    await brw.EvaluateScriptAsync("console.log(phuongthuc)");
                    phuongthuc = brw.ConsoleOutput;
                    //phuongthuc += brw.ConsoleOutput;
                    //Tìm số input của từng form 
                    await brw.EvaluateScriptAsync("var soinput");
                    await brw.EvaluateScriptAsync(
                            "soinput = document.forms[" + i + "].getElementsByTagName('input').length");
                    await brw.EvaluateScriptAsync("console.log(soinput)");
                    int soinput = Int32.Parse(brw.ConsoleOutput);
                    //phuongthuc += "----" + soinput;
                    //chèn script vào từng input
                    if (phuongthuc.ToString() == "get")
                    {
                        for (int j = 0; j < soinput; j++)
                        {
                            await brw.EvaluateScriptAsync(
                                    "document.forms[" + i + "].getElementsByTagName('input')[" + j + "].value='" + script + "'");
                        }

                        if (await SubmitForm(brw, i) == "Hello World")
                        {
                            kq = "Co lo hong XSS tai form thu "+i;
                        }
                        bool loadagainurl = await loadurl(brw, url);
                    }
                }



            return url + " --------- " + kq;
        }

        //load url và chờ nó load xong mới return hàm
        public static async Task<bool> loadurl(this ChromiumWithScript brw,String url)
        {
            brw.Load(url);
            var isBrwDoneLoading = false;
            brw.LoadingStateChanged += BrwOnLoadingStateChanged;
            //Local
            async void BrwOnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
            {
                // ReSharper disable once AccessToModifiedClosure
                if (e.IsLoading || isBrwDoneLoading) return;
                isBrwDoneLoading = true;
            }
            while (isBrwDoneLoading == false)
            {
                await Task.Delay(1);
            }
            return isBrwDoneLoading;
        }

        // submit form số i
        public static async Task<String> SubmitForm(this ChromiumWithScript brw, int i)
        {
            await brw.EvaluateScriptAsync("document.forms[" + i + "].submit()");
            var isBrwDoneLoading = false;
            brw.LoadingStateChanged += BrwOnLoadingStateChanged;
            //Local
            async void BrwOnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
            {
                // ReSharper disable once AccessToModifiedClosure
                if (e.IsLoading || isBrwDoneLoading) return;
                isBrwDoneLoading = true;
            }
            while(isBrwDoneLoading == false)
            {
                await Task.Delay(1);
            }
            return brw.ConsoleOutput.ToString();
        }
    }
}
