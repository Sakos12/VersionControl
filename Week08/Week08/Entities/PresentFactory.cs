using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week08.Abstractions;

namespace Week08.Entities
{
    public class PresentFactory: IToyFactory
    {
        public Toy CreateNew()
        {
            return new Present();
        }
    }
}
