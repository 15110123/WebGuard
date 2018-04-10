using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WebGuard.Supplier
{
    public static class BitmapExtension
    {
        public static ChromiumWebBrowser To(this Bitmap bitmap, IList<Bitmap> bitmaps, ChromiumWebBrowser brw)
        {
            bitmaps.Add(bitmap);
            return brw;
        }

        public static Bitmap MergeIntoOneBitmap(this IList<Bitmap> bitmaps)
        {
            //Get bitmap width and height
            var width = bitmaps[0].Width;
            var viewHeight = bitmaps[0].Height;
            var height = viewHeight * bitmaps.Count;

            var bitmap = new Bitmap(width, height);
            using (var gp = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < bitmaps.Count; i++)
                {
                    gp.DrawImage(bitmaps[i], new Point(0, viewHeight * i));
                }
            }

            return bitmap;
        }

        public static string SaveTo(this Bitmap bitmap, string path)
        {
            Directory.CreateDirectory(path);
            var filepath = $@"{path}\{Guid.NewGuid().ToString()}.jpg";
            bitmap.Save(filepath);
            return filepath;
        }
    }
}
