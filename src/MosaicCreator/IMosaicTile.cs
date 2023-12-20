using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public interface IMosaicTile
    {
        public void DrawOn(Graphics graphics, Size graphicsSize);
    }
}
