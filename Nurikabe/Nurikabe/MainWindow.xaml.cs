using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Markup;

namespace Nurikabe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Viewbox viewbox;
        private Canvas canvas;
        Options options;

        public MainWindow()
        {
            InitializeComponent();
        }

        //string color = blocks[i, j].Center == false ? : "B" : "W";
        public void InitializeBoard(BlockStruct[,] blocks, int n)
        {
            mainGrid.ShowGridLines = true;
            TextBlock[,] block = new TextBlock[n,n];
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
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
                    if (blocks[i,j].Center == false)
                    {
                        color = "B";
                    }
                    else { color = ""; }
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
    }
}