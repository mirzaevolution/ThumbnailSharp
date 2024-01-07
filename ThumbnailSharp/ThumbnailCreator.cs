/*MIT License

Copyright(c) 2017 Mirza Ghulam Rasyid

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace ThumbnailSharp
{
    /// <summary>
    /// Image format to use when creating a thumbnail.
    /// </summary>
    public enum Format
    {
        Jpeg,
        Bmp,
        Png,
        Gif,
        Tiff

    }
    /// <summary>
    /// make thumbnail force by ratio
    /// </summary>
    public enum Ratio
    {

        Landscape,
        Portrait
    }
    /// <summary>
    /// Thumbnail class that holds various methods to create an image thumbnail.
    /// </summary>
    public class ThumbnailCreator
    {
        Ratio? forceRatio = null;
        public ThumbnailCreator() : this(null)
        {
        }
        public ThumbnailCreator(Ratio? forceRatio)
        {

            this.forceRatio = forceRatio;
        }

        private Stream GetStreamFromFileLocation(string imageLocation)
        {
            if (!File.Exists(imageLocation))
            {
                throw new FileNotFoundException("fileNotFound:" + imageLocation);
            }
            return File.OpenRead(imageLocation);
        }
        Ratio GetRatio(Ratio? forceRatio, float actualWidth, float actualHeight)
        {
            if (forceRatio.HasValue)
            {
                return forceRatio.Value;
            }
            return actualWidth >= actualHeight ? Ratio.Landscape : Ratio.Portrait;

        }
        private Bitmap CreateBitmapThumbnail(uint thumbnailSize, string imageFileLocation)
        {
            return CreateBitmapThumbnail(thumbnailSize, GetStreamFromFileLocation(imageFileLocation));
            Bitmap bitmap = null;
            Image image = null;
            float actualHeight = default(float);
            float actualWidth = default(float);
            uint thumbnailHeight = default(uint);
            uint thumbnailWidth = default(uint);
            try
            {
                image = Image.FromFile(imageFileLocation);
            }
            catch
            {
                if (image != null)
                    image = null;
            }
            if (image != null)
            {
                actualHeight = image.Height;
                actualWidth = image.Width;
                if (forceRatio.Value == Ratio.Portrait || actualHeight > actualWidth)
                {
                    if ((uint)actualHeight <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than actual height (portrait image)");
                    thumbnailHeight = thumbnailSize;
                    thumbnailWidth = (uint)((actualWidth / actualHeight) * thumbnailSize);
                }
                else if (forceRatio.Value == Ratio.Landscape && actualWidth > actualHeight)
                {

                    if ((uint)actualWidth <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than actual width (landscape image)");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = (uint)((actualHeight / actualWidth) * thumbnailSize);
                }
                else
                {
                    if ((uint)actualWidth <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than image's size");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = thumbnailSize;
                }
                try
                {

                    bitmap = new Bitmap((int)thumbnailWidth, (int)thumbnailHeight);
                    Graphics resizedImage = Graphics.FromImage(bitmap);
                    resizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    resizedImage.CompositingQuality = CompositingQuality.HighQuality;
                    resizedImage.SmoothingMode = SmoothingMode.HighQuality;
                    resizedImage.DrawImage(image, 0, 0, thumbnailWidth, thumbnailHeight);
                }
                catch
                {
                    if (bitmap != null)
                        bitmap = null;
                }
            }
            return bitmap;
        }
        private Bitmap CreateBitmapThumbnail(uint thumbnailSize, Stream imageStream)
        {
            Bitmap bitmap = null;
            System.Drawing.Image image = null;
            float actualHeight = default(float);
            float actualWidth = default(float);
            uint thumbnailHeight = default(uint);
            uint thumbnailWidth = default(uint);
            try
            {
                image = Image.FromStream(imageStream);
            }
            catch
            {
                if (image != null)
                    image = null;
            }
            if (image != null)
            {
                actualHeight = image.Height;
                actualWidth = image.Width;
                if (GetRatio(forceRatio,actualWidth, actualHeight)== Ratio.Portrait)
                {
                    if ((uint)actualHeight <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than actual height (portrait image)");
                    thumbnailHeight = thumbnailSize;
                    thumbnailWidth = (uint)((actualWidth / actualHeight) * thumbnailSize);
                }
                else if (GetRatio(forceRatio, actualWidth, actualHeight) == Ratio.Landscape)
                {

                    if ((uint)actualWidth <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than actual width (landscape image)");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = (uint)((actualHeight / actualWidth) * thumbnailSize);
                }
                else
                {
                    if ((uint)actualWidth <= thumbnailSize)
                        return (Bitmap)image;
                    //throw new Exception("Thumbnail size must be less than image's size");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = thumbnailSize;
                }
                try
                {
                    bitmap = new Bitmap((int)thumbnailWidth, (int)thumbnailHeight);
                    Graphics resizedImage = Graphics.FromImage(bitmap);
                    resizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    resizedImage.CompositingQuality = CompositingQuality.HighQuality;
                    resizedImage.SmoothingMode = SmoothingMode.HighQuality;
                    resizedImage.DrawImage(image, 0, 0, thumbnailWidth, thumbnailHeight);
                }
                catch
                {
                    if (bitmap != null)
                        bitmap = null;
                }
            }
            return bitmap;
        }
        private ImageFormat GetImageFormat(Format format)
        {
            switch (format)
            {
                case Format.Jpeg:
                    return ImageFormat.Jpeg;
                case Format.Bmp:
                    return ImageFormat.Bmp;
                case Format.Png:
                    return ImageFormat.Png;
                case Format.Gif:
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Tiff;
            }
        }
        private async Task<Stream> GetImageStreamFromUrl(Uri urlAddress)
        {
            Stream result = null;
            try
            {
                byte[] bytes = await GetImageBytesFromUrl(urlAddress);
                result = new MemoryStream(bytes);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        private async Task<byte[]> GetImageBytesFromUrl(Uri urlAddress)
        {
            byte[] buffer = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    buffer = await client.GetByteArrayAsync(urlAddress);
                }
            }
            catch
            {
                buffer = null;
            }
            return buffer;
        }

        /// <summary>
        /// Create a thumbnail from file and returns as stream.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageFileLocation">Correct image file location.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as stream. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageFileLocation' is null.</exception>
        /// <exception cref="FileNotFoundException">'imageFileLocation' does not exist.</exception>
        public Stream CreateThumbnailStream(uint thumbnailSize, string imageFileLocation, Format imageFormat)
        {
            if (String.IsNullOrEmpty(imageFileLocation))
                throw new ArgumentNullException(nameof(imageFileLocation), "'imageFileLocation' cannot be null");
            if (!File.Exists(imageFileLocation))
                throw new FileNotFoundException($"'{imageFileLocation}' cannot be found");
            Bitmap bitmap = CreateBitmapThumbnail(thumbnailSize, imageFileLocation);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, GetImageFormat(imageFormat));
                stream.Position = 0;
                return stream;
            }
            return null;
        }
        /// <summary>
        /// Create a thumbnail from image stream and returns as stream.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageStream">Valid image stream object.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as stream. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageStream' is null.</exception>
        public Stream CreateThumbnailStream(uint thumbnailSize, Stream imageStream, Format imageFormat)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream), "'imageStream' cannot be null");
            Bitmap bitmap = CreateBitmapThumbnail(thumbnailSize, imageStream);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, GetImageFormat(imageFormat));
                stream.Position = 0;
                return stream;
            }
            return null;
        }
        /// <summary>
        /// Create a thumbnail from image in bytes and returns as stream.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageBytes">Valid image bytes array.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as stream. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageBytes' is null.</exception>
        public Stream CreateThumbnailStream(uint thumbnailSize, byte[] imageBytes, Format imageFormat)
        {
            if (imageBytes == null)
                throw new ArgumentNullException(nameof(imageBytes), "'imageStream' cannot be null");
            Bitmap bitmap = CreateBitmapThumbnail(thumbnailSize, new MemoryStream(imageBytes));
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, GetImageFormat(imageFormat));
                stream.Position = 0;
                return stream;
            }
            return null;
        }
        /// <summary>
        /// Create a thumbnail from file and returns as bytes.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageFileLocation">Correct image file location.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as bytes. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageFileLocation' is null.</exception>
        /// <exception cref="FileNotFoundException">'imageFileLocation' does not exist.</exception>
        public byte[] CreateThumbnailBytes(uint thumbnailSize, string imageFileLocation, Format imageFormat)
        {
            if (String.IsNullOrEmpty(imageFileLocation))
                throw new ArgumentNullException(nameof(imageFileLocation), "'imageFileLocation' cannot be null");
            if (!File.Exists(imageFileLocation))
                throw new FileNotFoundException($"'{imageFileLocation}' cannot be found");
            Stream stream = CreateThumbnailStream(thumbnailSize, imageFileLocation, imageFormat);
            if (stream != null)
            {
                byte[] streamBytes = new byte[stream.Length];
                stream.Read(streamBytes, 0, streamBytes.Length);
                return streamBytes;
            }
            return null;
        }
        /// <summary>
        /// Create a thumbnail from image stream and returns as bytes.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageStream">Valid image stream object.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as bytes. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageStream' is null.</exception>
        public byte[] CreateThumbnailBytes(uint thumbnailSize, Stream imageStream, Format imageFormat)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream), "'imageStream' cannot be null");

            Stream stream = CreateThumbnailStream(thumbnailSize, imageStream, imageFormat);
            if (stream != null)
            {
                byte[] streamBytes = new byte[stream.Length];
                stream.Read(streamBytes, 0, streamBytes.Length);
                return streamBytes;
            }
            return null;
        }
        /// <summary>
        /// Create a thumbnail from image in bytes and returns as bytes.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="imageBytes">Valid image bytes array.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as bytes. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'imageBytes' is null.</exception>
        public byte[] CreateThumbnailBytes(uint thumbnailSize, byte[] imageBytes, Format imageFormat)
        {
            if (imageBytes == null)
                throw new ArgumentNullException(nameof(imageBytes), "'imageStream' cannot be null");
            Stream stream = CreateThumbnailStream(thumbnailSize, imageBytes, imageFormat);
            if (stream != null)
            {
                byte[] streamBytes = new byte[stream.Length];
                stream.Read(streamBytes, 0, streamBytes.Length);
                return streamBytes;
            }
            return null;
        }


        /// <summary>
        /// Create a thumbnail from valid image url asynchronously.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="urlAddress">Valid absolute url address with proper scheme.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as stream. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'urlAddress' is null.</exception>
        public async Task<Stream> CreateThumbnailStreamAsync(uint thumbnailSize, Uri urlAddress, Format imageFormat)
        {
            if (urlAddress == null)
                throw new ArgumentNullException(nameof(urlAddress), "'urlAddress' cannot be null");
            Stream result = null;
            Stream stream = await GetImageStreamFromUrl(urlAddress);
            if (stream != null)
            {
                result = CreateThumbnailStream(thumbnailSize, stream, imageFormat);
            }
            return result;
        }

        /// <summary>
        /// Create a thumbnail from valid image url asynchronously.
        /// </summary>
        /// <param name="thumbnailSize">Thumbnail size. For portrait image, thumbnail size must be less than its height. 
        /// For landscape image, thumbnail size must be less than its width. For the same size image (Proportional), thumbnail size must be less than its width and height.</param>
        /// <param name="urlAddress">Valid absolute url address with proper scheme.</param>
        /// <param name="imageFormat">Image format to use.</param>
        /// <returns>A thumbnail image as bytes. Returns null if it fails.</returns>
        /// <exception cref="ArgumentNullException">'urlAddress' is null.</exception>
        public async Task<byte[]> CreateThumbnailBytesAsync(uint thumbnailSize, Uri urlAddress, Format imageFormat)
        {
            if (urlAddress == null)
                throw new ArgumentNullException(nameof(urlAddress), "'urlAddress' cannot be null");
            byte[] result = null;
            byte[] imageBytes = await GetImageBytesFromUrl(urlAddress);
            if (imageBytes != null)
            {
                result = CreateThumbnailBytes(thumbnailSize, imageBytes, imageFormat);
            }
            return result;
        }



    }
}
