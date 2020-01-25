using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GLSLWallpapers.Helpers {
    public static class Converters {
        public static string ImageToBase64Url(Image image) {
            using MemoryStream stream = new MemoryStream();
            image.Save(stream, image.RawFormat);
            ImageCodecInfo info = ImageCodecInfo.GetImageDecoders().First(codecInfo => codecInfo.FormatID == image.RawFormat.Guid);
            return $"data:{info.MimeType};base64,{Convert.ToBase64String(stream.ToArray())}";
        }
    }
}
