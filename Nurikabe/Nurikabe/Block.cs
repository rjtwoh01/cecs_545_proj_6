using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurikabe
{
    class Block
    {
        public bool? Center;
        public bool? Up;
        public bool? Down;
        public bool? Right;
        public bool? Left;
        // Constructor that takes no arguments:
        public Block()
        {
        }
        //Gets the number of white blocks for new puzzel
        public static int NumberOfWhites(int n)
        {
            int numWhite = Convert.ToInt32((n * n) * .44);
            return numWhite;
        }
        public static List<Block> GetPuzzel(int n)
        {
            List<bool> list = new List<bool>();
            int numWhite = NumberOfWhites(n);
            List<Block> blocks = new List<Block>();
            //need a two diminsional array to get the blocks
            for (int i = 0; i < numWhite; i++)
            {
                list.Add(true);
            }
            for (int i = 0; i < n * n - numWhite; i++)
            {
                list.Add(false);
            }
            Random ran = new Random();
            //i is row
            for (int i = 0; i < n; i++)
            {
                //j is column
                for (int j = 0; j < n; j++)
                {
                    int index = ran.Next(list.Count);
                    //Block[i][j] = list(index);
                    //list.
                }
            }
            return blocks;
        }
    }
}
