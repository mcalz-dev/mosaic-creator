using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class Configuration
    {

        public string InputImagePath { get; set; } = string.Empty;

        public string OutputImagePath { get; set; } = string.Empty;

        public string MosaicTilesDirectory { get; set; } = string.Empty;

        public string WorkingDirectory { get; set; } = string.Empty;
    }
}
