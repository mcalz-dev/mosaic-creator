using System.Collections;
using System.Drawing;

namespace MosaicCreator
{
    internal class BitmapEnumerator : IEnumerator<Color>
    {
        private Bitmap _bitmap;
        private int positionX = -1;
        private int positionY = 0;

        public BitmapEnumerator(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public Color Current => _bitmap.GetPixel(positionX, positionY);
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _bitmap = null!;
        }

        public bool MoveNext()
        {
            positionX++;

            if (positionX >= _bitmap.Width)
            {
                positionX = 0;
                positionY++;

                if (positionY >= _bitmap.Height)
                {
                    return false;
                }
            }

            return true;
        }

        public void Reset()
        {
            positionX = -1;
            positionY = 0;
        }
    }

}
