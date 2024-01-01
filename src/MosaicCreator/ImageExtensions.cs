using System.Drawing.Drawing2D;
using System.Drawing;
using System.Security.Cryptography;

namespace MosaicCreator
{
    internal static class ImageExtensions
    {
        public static double GetAspectRatio(this Image image)
        {
            return (double)image.Width / image.Height;
        }

        public static Bitmap Scale(this Image originalImage, Size maxSize, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
        {
            var newSize = CalculateNewSize(originalImage.Size, maxSize);
            return Resize(originalImage, newSize, interpolationMode);
        }

        public static Bitmap Resize(this Image originalImage, Size newSize, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
        {
            var resizedImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
            }

            return resizedImage;
        }

        public static IReadOnlyList<Color> GetPixels(this Bitmap bitmap)
        {
            var pixels = new List<Color>();
            foreach (var pixel in bitmap)
            {
                pixels.Add(pixel);
            }

            return pixels;
        }

        public static IEnumerator<Color> GetEnumerator(this Bitmap bitmap)
        {
            return new BitmapEnumerator(bitmap);
        }

        public static byte[] GetHash(this Bitmap bitmap)
        {
            var ic = new ImageConverter();
            var bytes = (byte[])ic.ConvertTo(bitmap, typeof(byte[]))!;
            return SHA256.HashData(bytes);
        }

        private static Size CalculateNewSize(Size originalSize, Size maxDimensions)
        {
            double aspectRatio = originalSize.GetAspectRatio();

            int newWidth, newHeight;

            if (originalSize.Width > maxDimensions.Width || originalSize.Height > maxDimensions.Height)
            {
                if (aspectRatio > 1)
                {
                    newWidth = maxDimensions.Width;
                    newHeight = (int)(maxDimensions.Width / aspectRatio);
                }
                else
                {
                    newWidth = (int)(maxDimensions.Height * aspectRatio);
                    newHeight = maxDimensions.Height;
                }
            }
            else
            {
                newWidth = originalSize.Width;
                newHeight = originalSize.Height;
            }

            return new Size(Math.Max(1, newWidth), Math.Max(1, newHeight));
        }
    }
}
