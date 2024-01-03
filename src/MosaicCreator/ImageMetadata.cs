using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace MosaicCreator
{
    internal struct ImageMetadata : IEquatable<ImageMetadata>
    {
        private ImageMetadata(Size size, Pictogram pictogram, ColorHistogram colorHistogram, byte[] hash)
        {
            AspectRatio = size.GetAspectRatio();
            Size = size;
            Pictogram = pictogram;
            ColorHistogram = colorHistogram;
            Hash = hash;
        }

        internal Pictogram Pictogram { get; }

        internal ColorHistogram ColorHistogram { get; }

        internal Size Size { get; }

        internal double AspectRatio { get; }

        internal byte[] Hash { get; }

        internal static ImageMetadata Of(Bitmap image)
        {
            return new ImageMetadata(image.Size, Pictogram.Of(image), ColorHistogram.Of(image), image.GetHash());
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null || obj is not  ImageMetadata) return false;
            return Equals((ImageMetadata)obj);
        }

        public bool Equals(ImageMetadata other)
        {
            return Equals(Hash, other.Hash);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}
