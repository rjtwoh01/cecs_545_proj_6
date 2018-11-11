using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Nurikabe
{
    class NurikabeSolve
    {
        public static BlockStruct[,] blocks;
        private RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();

        public List<BlockStruct> FisherYatesShuffle()
        {
            List<BlockStruct> shuffledList = blocks.OfType<BlockStruct>().ToList();

            int n = shuffledList.Count;

            while (n > 1)
            {
                var box = new byte[1];
                do this.cryptoProvider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            return shuffledList;
        }

        public int Fitness(BlockStruct[,] grid, int row, int n)
        {
            int neighbors = 0;
            int fitness = 0;
            for (int i = 0; i < n; i++)
            {
                if (grid[row + 1, i].Center == false)  //it's black directly above
                {
                    neighbors++;
                }
                if (grid[row - 1,i].Center == false) //it's black directly below
                {
                    neighbors++;
                }
                if (grid[row,i - 1].Center == false || i == 0)  //it's black directly left or it is an edge
                {
                    neighbors++;
                }
                if (grid[row - 1,i].Center == false || i == n - 1) //it's black directly to the right or at an edge
                {
                    neighbors++;
                }
                if(neighbors == 2||neighbors == 3)  //if it has 2 or 3 neighbors it is more likley to produce a valid solution
                {
                    fitness++;
                }
            }
            return fitness;
        }

        public static void CheckNeighbors(BlockStruct[][] blocks, ref List<BlockStruct> islandList, int i, int j, int n)
        {
            if (i != 0)  //not at top row check above it
            {
                if (blocks[i + 1][j].Center == true && blocks[i + 1][j].Visited == false)
                {
                    islandList.Add(blocks[i + 1][j]);
                    blocks[i + 1][j].Visited = true;
                }
            }
            if (i != n - 1)  //not the last row
            {
                if (blocks[i - 1][j].Center == true && blocks[i - 1][j].Visited == false)
                {
                    islandList.Add(blocks[i - 1][j]);
                    blocks[i - 1][j].Visited = true;
                }
            }
            if (j != 0)  //not at the left end
            {
                if (blocks[i][j - 1].Center == true && blocks[i][j - 1].Visited == false)
                {
                    islandList.Add(blocks[i][j - 1]);
                    blocks[i][j - 1].Visited = true;
                }
            }
            if (j != n - 1)  //not at the right end
            {
                if (blocks[i][j + 1].Center == true && blocks[i][j + 1].Visited == false)
                {
                    islandList.Add(blocks[i][j + 1]);
                    blocks[i][j + 1].Visited = true;
                }
            }
            islandList.RemoveAt(0); //remove top block from list
        }

        public static void IslandCounter(BlockStruct[][] blocks, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (blocks[i][j].Center == true && blocks[i][j].Visited == false)  //a white block found and not visited
                    {
                        List<BlockStruct> islandList = new List<BlockStruct>();
                        islandList.Add(blocks[i][j]);    //list to store the island list
                        blocks[i][j].Visited = true;
                        int counter = 1;
                        while (islandList.Count != 0)  //list of islands is not empty
                        {
                            CheckNeighbors(blocks, ref islandList, i, j, n);
                            counter++;
                        }
                        blocks[i][j].IslandValue = counter;
                    }
                }
            } 
        }


        public List<BlockStruct> Mutate(int n)
        {
            //Get random number for row mutation
            Random rnd = new Random();
            int row = rnd.Next(n);

            //variables for mutate function
            List<BlockStruct> bestChild = new List<BlockStruct>();
            int fitness = 0;
            int fittest = 0;

            //check WOC squares Counter
            for (int r = 0; r < n; r++)
            {
                //Needs to be blocks[r, x]... don't know how you want to do this yet
                //if (blocks[r].Counter > 10) //if a black square has been in this location at least 10 times, don't move it again
                //{
                //    blocks[r].StayPut = true;
                //}
            }

            List<BlockStruct> rowList = new List<BlockStruct>();

            //Collect the row block information from parent;
            for (int i = 0; i < n; i++)
            {
                rowList[i] = blocks[row,i];
            }

            for (int j = 0; j < 10; j++) //collect 10 children
            {
                //List<BlockStruct> child = ShuffleList<BlockStruct>(n);
                List<BlockStruct> temp = blocks.OfType<BlockStruct>().ToList();
                //List<BlockStruct> child = genericFisherYatesShuffle<BlockStruct>(temp);
                List<BlockStruct> child = FisherYatesShuffle();
                bool invalidChild = false;

                //check WOC squares Counter for valid child if invalid it will be repeated in the below while loop
                for (int r = 0; r < n; r++)
                {
                    bool stayPut = blocks[row, r].StayPut ?? false;
                    bool center = child[r].Center ?? false;
                    if (stayPut && center != false)  //Invalid Child
                    {
                        invalidChild = true;
                    }
                }

                while (invalidChild == true)  //will break out once a valid child is found for WOC approach
                {
                    invalidChild = false;  //reset the flag
                    //child = ShuffleList<BlockStruct>(n);
                    //check WOC squares Counter
                    for (int r = 0; r < n; r++)  //check the row against parent counters
                    {
                        bool stayPut = blocks[row, r].StayPut ?? false;
                        bool center = child[r].Center ?? false;
                        if (stayPut && center != false)  //Invalid Child
                        {
                            invalidChild = true;
                        }
                    }
                }

                if (j == 0) //first row put in bestChild
                {
                    bestChild = child;
                }

                //add back in to block structure for fitness check
                for (int k = 0; k < n; k++)
                {
                    blocks[row,k] = bestChild[k];
                }

                //check fitness of each child
                fitness = Fitness(blocks, row, n);

                if (fitness > fittest)
                {
                    bestChild = child;
                    fittest = fitness;
                }
            }

            //add bestchild row back in to block structure
            //mark black squares in the grid with a marker for WOC
            for (int k = 0; k < n; k++)
            {
                blocks[row,k] = bestChild[k];
                if (blocks[row,k].Center == false) //its black need to increment the counter
                {
                    blocks[row,k].Counter++;
                }
            }

            return bestChild;
        }
        
        //Need to do the two checks here 1 for ponds, 2 for connected sea (black squares) in RuleCheckHelper

        
    }
}
