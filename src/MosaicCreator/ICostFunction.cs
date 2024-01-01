using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal interface ICostFunction
    {
        /// <summary>
        /// Calculates the cost for drawing the source image over the destination section.
        /// </summary>
        /// <param name="source">The source image which may be applied to the destination.</param>
        /// <param name="destinationSection">A section of the destination where the modification should be applied to.</param>
        /// <returns>
        /// A value from 0.0 to 1.0, where 0.0 indicates no cost and 1.0 the maximum cost.
        /// </returns>
        double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection);

        void HandleWinner(ImageMetadata winner);
    }
}
