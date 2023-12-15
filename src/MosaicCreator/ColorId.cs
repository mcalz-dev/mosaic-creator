using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal struct ColorId
    {
        public static readonly ColorId MinValue = new ColorId(0);
        public static readonly ColorId MaxValue = new ColorId(255);

        private ColorId(int id)
        {
            Id = id;
        }


        public static ColorId FromValue(int id)
        {
            if (id < MinValue.Id | id > MaxValue.Id)
            {
                throw new ArgumentOutOfRangeException($"Id {id} is out of range of allowed values [{MinValue.Id},{MaxValue.Id}]");
            }

            return new ColorId(id);
        }

        public int Id { get; }
    }
}
