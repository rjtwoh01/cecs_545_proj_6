using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Nurikabe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Options options;
        bool hasNurikabeRun;
        BlockStruct[,] blocks;
        int n;
        int iterations;
        int wocVisit;

        public MainWindow()
        {
            InitializeComponent();
            hasNurikabeRun = false;
        }

        //string color = blocks[i, j].Center == false ? : "B" : "W";
        public void InitializeBoard(BlockStruct[,] blocks, int n, int iterations, int wocVisit)
        {
            this.blocks = blocks;
            this.n = n;
            this.iterations = iterations;
            this.wocVisit = wocVisit;
            mainGrid.ShowGridLines = true;
            TextBlock[,] block = new TextBlock[n, n];
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.Children.Clear();
            for (int i = 0; i < n; i++)
            {
                RowDefinition row = new RowDefinition();
                ColumnDefinition col = new ColumnDefinition();

                mainGrid.ColumnDefinitions.Add(col);
                mainGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < n; i++)
            {

                for (int j = 0; j < n; j++)
                {
                    string color = "W";
                    if (blocks[i, j].Center == false)
                    {
                        color = "B";
                    }
                    else { if (blocks[i, j].IslandValue != 0) color = blocks[i, j].IslandValue.ToString(); else color = ""; }
                    var border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0.1, 0.1, 0.1, 0.1)
                    };
                    block[i, j] = new TextBlock
                    {
                        FontSize = 15,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = color
                    };
                    border.Child = block[i, j];
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    mainGrid.Children.Add(border);
                }
            }

            if (hasNurikabeRun == false)
            {
                //runNurikabe(blocks, n, iterations, wocVisit);
            }
        }

        public void runNurikabe()
        {
            NurikabeSolve nurikabe = new NurikabeSolve();
            Random rand = new Random();
            hasNurikabeRun = true;
            BlockStruct[,] temp = blocks;
            Stopwatch sw = Stopwatch.StartNew();
            List<TimeSpan> elapsedTime = new List<TimeSpan>();
            List<int> fitness = new List<int>();
            for (int i = 0; i < iterations; i++)
            {
                Stopwatch innerSw = Stopwatch.StartNew();
                int fittest = 0;
                temp = nurikabe.Mutate(temp, n, wocVisit, ref fittest);
                if (RuleCheckHelper.CheckPond(ref temp, n) == true && RuleCheckHelper.CheckSeaConncetion(temp, n) == true) { Debug.WriteLine("Success at {0}", i); break; }
                //InitializeBoard(temp, n, iterations, wocVisit);
                //Debug.WriteLine("Iteration {0}", i);
                innerSw.Stop();
                elapsedTime.Add(innerSw.Elapsed);
                fitness.Add(fittest);
            }
            sw.Stop();
            nurikabe.IslandCounter(ref temp, n);
            InitializeBoard(temp, n, iterations, wocVisit);
            hasNurikabeRun = false;
            writeFile(fitness, elapsedTime);
        }

        public void writeFile(List<int> fitness, List<TimeSpan> elapsedTime)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("C:/temp/nurikabe_board-size-" + n + "_iterations-" + iterations + "_wocVisits-" + wocVisit + ".csv"))
                {
                    writer.WriteLine("Iteration,Fitness,Elapsed Time");
                    for (int i = 0; i < iterations; i++)
                    {
                        writer.WriteLine(i + "," + fitness.ElementAt(i) + "," + elapsedTime.ElementAt(i).ToString("G"));
                    }
                }
            }
            catch (IOException e) {
                MessageBox.Show("Please close the open file");
                Debug.WriteLine(e);
            }
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            //mainGrid.Children.Clear();
            if (this.options == null)
            {
                this.options = new Options(this);
            }

            this.options.Show();
        }

        private void ClearScreen()
        {
            mainGrid.Children.Clear();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            runNurikabe();
        }
    }
}