using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurikabe
{
    public static class InitializationHelper
    {
        public static BlockStruct[,] InitializeBlockArray(int n)
        {
            //blocks[row,col]
            BlockStruct[,] blocks = new BlockStruct[n, n];

            int numOfWhite = (int)((n * n) * 0.44);
            List<bool> list = new List<bool>();

            //need a two diminsional array to get the blocks
            for (int i = 0; i < numOfWhite; i++)
            {
                list.Add(true);
            }
            for (int i = 0; i < n * n - numOfWhite; i++)
            {
                list.Add(false);
            }

            Random ran = new Random();
            int index;
            //i is row
            for (int i = 0; i < n; i++)
            {
                //j is column
                for (int j = 0; j < n; j++)
                {
                    index = ran.Next(list.Count);
                    blocks[i,j].Center = list[index];
                    list.RemoveAt(index);
                }
            }

            CheckAllWhiteRow(ref blocks);

            return blocks;
        }

        private static void CheckAllWhiteRow(ref BlockStruct[,] blocks)
        {
            for (int row = 0; row <= blocks.GetUpperBound(0); row++)
            {
                int whiteCounter = 0;
                for (int col = 0; col <= blocks.GetUpperBound(1); col++)
                {
                    if (blocks[row, col].Center == true)
                    {
                        whiteCounter++;
                    }
                }

                if (whiteCounter == blocks.GetUpperBound(1))
                {
                    //this row is all white, randomly change one cell to black
                    Random ran = new Random();
                    blocks[row, ran.Next(blocks.GetUpperBound(1))].Center = false;
                }
            }
        }
    }
}
