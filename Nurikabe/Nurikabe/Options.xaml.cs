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
using System.Windows.Shapes;
using System.Diagnostics;

namespace Nurikabe
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private MainWindow mainWindow;
        private bool forceClose = false;

        public Options(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.forceClose)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        public void ForceClose()
        {
            this.forceClose = true;
            this.Close();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            BlockStruct[,] blocks = InitializationHelper.InitializeBlockArray(Convert.ToInt32(txtBoxSize.Text));
            int i = 0;
            foreach (BlockStruct block in blocks)
            {
                string color = block.Center == false ? "black" : "white";
                Debug.WriteLine(i + ": " + color + "(" + block.Center + ")");
                i++;
            }

            mainWindow.InitializeBoard(blocks, Convert.ToInt32(txtBoxSize.Text));
            this.Hide();
        }
    }
}
