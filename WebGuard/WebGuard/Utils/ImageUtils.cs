using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

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
    }
}
