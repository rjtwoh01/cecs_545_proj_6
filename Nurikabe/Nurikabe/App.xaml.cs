using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Nurikabe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static BlockStruct[,] blocks;


        public static int Fitness(BlockStruct[,] grid, int row, int n)
        {
            int fitness = 0;
            for(int i =0; i<n;i++)
            {
                if(grid[row+1][i].Center == false)  //it's black directly above
                {
                    fitness++;
                }
                if(grid[row-1][i].Center == false) //it's black directly below
                {
                    fitness++;
                }
                if(grid[row][i-1].Center == false || i == 0)  //it's black directly left or it is an edge
                {
                    fitness++;
                }
                if(grid[row-1][i].Center == false || i == n-1) //it's black directly to the right or at an edge
                {
                    fitness++;
                }
            }
            return fitness;
        }

        public static List<BlockStruct> Mutate(int n) 
        {
            //Get random number for row mutation
            Random rnd = new Random();
            int row = rnd.Next(n);

            //variables for mutate function
            List<BlockStruct> bestChild = new List<BlockStruct>();
            int fitness = 0;
            int fittest = 0;

            //check WOC squares Counter
                 for(int r = 0; r<n; r++)
                 {
                 if(blocks[i].Counter > 10) //if a black square has been in this location at least 10 times, don't move it again
                    {
                        blocks[i].StayPut == true;
                    }                           
                 }

            List<BlockStruct> rowList= new List<BlockStruct>();

            //Collect the row block information from parent;
            for (int i = 0 ;i < n; i++ )
            {
                rowList[i] = blocks[row][i];
            }

            for (int j = 0; j < 10 ; j ++) //collect 10 children
            {
                List<BlockStruct> child = ShuffleList<BlockStruct>(n);
                bool invalidChild = false;

                //check WOC squares Counter for valid child if invalid it will be repeated in the below while loop
                 for(int r = 0; r<n; r++)
                 {
                    if(blocks[row][i].StayPut && child[i].Center != false)  //Invalid Child
                    {
                        invalidChild = true;
                    }                           
                 }
                 
                 while(invalidChild == true)  //will break out once a valid child is found for WOC approach
                 {
                    invalidChild == false;  //reset the flag
                    child = ShuffleList<BlockStruct>(n);
                     //check WOC squares Counter
                     for(int r = 0; r<n; r++)  //check the row against parent counters
                     {
                     if(blocks[row][i].StayPut && child[i].Center != false)  //Invalid Child
                        {
                        invalidChild = true;
                        }                           
                     }
                 }

                if(j == 0) //first row put in bestChild
                {
                bestChild = child;
                }

                 //add back in to block structure for fitness check
                 for (int k = 0; k< n; k++) 
                 {
                 blocks[row][k] = bestChild[i];
                 }

                 //check fitness of each child
                 fitness = Fitness(blocks, row, n);

                if(fitness > fittest)
                {
                bestChild = child;
                fittest = fitness;
                }
            }
            
        //add bestchild row back in to block structure
        //mark black squares in the grid with a marker for WOC
        for (int k = 0; k< n; k++)  
        {
            blocks[row][k] = bestChild[i];
                if(blocks[row][k] == false) //its black need to increment the counter
                    {
                     blocks[row][k].Counter++;
                    } 
        }
        }
        //Need to do the two checks here 1 for ponds, 2 for connected sea (black squares)

    }
}

