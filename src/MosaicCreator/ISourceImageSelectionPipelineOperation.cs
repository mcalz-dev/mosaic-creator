using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal interface ISourceImageSelectionPipelineOperation
    {
        IEnumerable<ISourceImage> Apply(IEnumerable<ISourceImage> sourceImages, ImageMetadata destinationMetadata);
    }
}
