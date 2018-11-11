using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurikabe
{
    public static class RuleCheckHelper
    {
        /// <summary>
        /// check if the graph exist a 2x2 pond
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>TRUE indicate that the graph has no pond, FALSE means there exist pond</returns>
        public static bool CheckPond(BlockStruct[,] blocks)
        {
            //the following steps is how to check if the graph exist the 2x2 pond
            //1. iterate each cell in the graph
            //2. if the cell is white, skip, continue iteration. if the cell is black, continue next step
            //3. check 4 diagonal cell(up-right, up-left, bottom-left, bottom-right). if one of the diagonal cell is black, then contine the next step
            //4. check the the 2 cells which are connected to both the diagonal cell and the center cell, for example, if the up-right is black, then check if the up and right cell are black. if all 4 cell are black, then there is a pond, terminate this method and return false 

            for (int row = 1; row <= blocks.GetUpperBound(0) - 1; row++)
            {
                for (int col = 1; col <= blocks.GetUpperBound(1) - 1; col++)
                {
                    //skip the white cell
                    if (blocks[row, col].Center == true)
                    {
                        continue;
                    }

                    //check 4 diagonal cells
                    if (blocks[row - 1, col + 1].Center == false)//up-right
                    {
                        if (blocks[row - 1, col].Center == false && blocks[row, col + 1].Center == false)
                        {
                            return false;
                        }
                    }
                    else if (blocks[row - 1, col - 1].Center == false)//up-left
                    {
                        if (blocks[row - 1, col].Center == false && blocks[row, col - 1].Center == false)
                        {
                            return false;
                        }
                    }
                    else if (blocks[row + 1, col + 1].Center == false)//bottom-right
                    {
                        if (blocks[row + 1, col].Center == false && blocks[row, col + 1].Center == false)
                        {
                            return false;
                        }
                    }
                    else if (blocks[row + 1, col - 1].Center == false)//bottom-left
                    {
                        if (blocks[row + 1, col].Center == false && blocks[row, col - 1].Center == false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the sea is connected
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>TRUE indicate the sea is properly connected, FALSE indicate the sea is cutted</returns>
        public static bool CheckSeaConncetion(BlockStruct[,] blocks)
        {
            for (int row = 0; row <= blocks.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= blocks.GetUpperBound(1); col++)
                {
                    //skip the white cell
                    if (blocks[row, col].Center == true)
                    {
                        continue;
                    }

                    //as long as a black cell is connected to another black cell, it's ok
                    if (blocks[row - 1, col].Center == false || blocks[row, col - 1].Center == false || blocks[row + 1, col].Center == false || blocks[row, col + 1].Center == false)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
