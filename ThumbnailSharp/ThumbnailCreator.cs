using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ThumbnailSharp
{
    /// <summary>
    /// Image format to use when creating a thumbnail
    /// </summary>
    public enum Format
    {
        Jpeg,
        Bmp,
        Png,
        Gif,
        Icon,
        Tiff,
        Exif,
        Wmf,
        Emf,
    }
    /// <summary>
    /// Thumbnail class that holds various methods to create an image thumbnail.
    /// </summary>
    public class ThumbnailCreator
    {
        private Bitmap CreateBitmapThumbnail(uint thumbnailSize, string imageFileLocation)
        {
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
                if (actualHeight > actualWidth)
                {
                    if ((uint)actualHeight <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than actual height (portrait image)");
                    thumbnailHeight = thumbnailSize;
                    thumbnailWidth = (uint)((actualWidth / actualHeight) * thumbnailSize);
                }
                else if (actualWidth > actualHeight)
                {

                    if ((uint)actualWidth <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than actual width (landscape image)");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = (uint)((actualHeight / actualWidth) * thumbnailSize);
                }
                else
                {
                    if ((uint)actualWidth <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than image's size");
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
            Image image = null;
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
                if (actualHeight > actualWidth)
                {
                    if ((uint)actualHeight <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than actual height (portrait image)");
                    thumbnailHeight = thumbnailSize;
                    thumbnailWidth = (uint)((actualWidth / actualHeight) * thumbnailSize);
                }
                else if (actualWidth > actualHeight)
                {

                    if ((uint)actualWidth <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than actual width (landscape image)");
                    thumbnailWidth = thumbnailSize;
                    thumbnailHeight = (uint)((actualHeight / actualWidth) * thumbnailSize);
                }
                else
                {
                    if ((uint)actualWidth <= thumbnailSize)
                        throw new Exception("Thumbnail size must be less than image's size");
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
            switch(format)
            {
                case Format.Jpeg:
                    return ImageFormat.Jpeg;
                case Format.Bmp:
                    return ImageFormat.Bmp;
                case Format.Png:
                    return ImageFormat.Png;
                case Format.Gif:
                    return ImageFormat.Gif;
                case Format.Icon:
                    return ImageFormat.Icon;
                case Format.Tiff:
                    return ImageFormat.Tiff;
                case Format.Exif:
                    return ImageFormat.Exif;
                case Format.Wmf:
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Emf;
            }
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

    }
}
