using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebGuard.Utils
{
    public static class ImageUtils
    {
        public static Bitmap SaveJpeg(this Bitmap img, string path, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            var jpegCodec = GetEncoderInfo("image/jpeg");
            var encoderParams = new EncoderParameters(1)
            {
                Param = { [0] = qualityParam }
            };
            img.Save(path, jpegCodec, encoderParams);
            return img;
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
            => ImageCodecInfo.GetImageEncoders()
                        .FirstOrDefault(t => t.MimeType == mimeType);

        public enum Method
        {
            Get, Post
        }

        public static async Task<Bitmap> DownloadBitmap(string url, Method method,
            params (string parameter, string value)[] content)
        {
            Stream stream;

            using (var httpClient = new HttpClient())
            {
                if (method == Method.Get)
                {
                    stream = await httpClient.GetStreamAsync(url);
                }
                else
                {
                    //var contentKeyValPair = content.Select(x => new KeyValuePair<string, string>(x.parameter, x.value));

                    //var httpContent = new FormUrlEncodedContent(contentKeyValPair);

                    foreach (var ele in content)
                    {
                        httpClient.DefaultRequestHeaders.Add(ele.parameter, ele.value);
                    }

                    stream = await (await httpClient.PostAsync(url, null))
                        .Content
                        .ReadAsStreamAsync();
                }
            }

            return new Bitmap(stream);
        }
    }
}
