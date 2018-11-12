using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurikabe
{
    public struct BlockStruct
    {
        public bool? Center; //Black or white node
        public int Counter;
        public bool? StayPut;
        public bool? Visited;
        public int IslandValue;
        public bool isVisited; //used for black connectivity check
    }
}
