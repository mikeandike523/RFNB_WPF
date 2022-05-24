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

namespace RFNB_UWP
{

    //Notes to self
    //1. I am not sure if I will use UWP or WPF. WPF Seems to have synchrounous serialization of inkstrokes, which is a plus.
    //https://docs.microsoft.com/en-us/dotnet/desktop/wpf/advanced/storing-ink?view=netframeworkdesktop-4.8
    //Attribution policy
    // 1. Attributions will only be given if significant portions of code are copy pasted or significant ideas are copied. Rewriting a few lines of code will likely not be attributed.
    // 2. Official documentation will not be attributed, unless a large block of code is copy-pasted.

    public partial class MainWindow : Window
    {

        //Notebook Function

        Notebook notebook = new Notebook("Untitled");


        //UI Function

        public MainWindow()
        {
            InitializeComponent();
        }

        private double initialX;
        private double initialY;
        private double currentX;
        private double currentY;
        private double finalX;
        private double finalY;

        private void MainCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            initialX = e.GetTouchPoint(null).Position.X;
            initialY = e.GetTouchPoint(null).Position.Y;
        }

        private void MainCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            currentX = e.GetTouchPoint(null).Position.X;
            currentY = e.GetTouchPoint(null).Position.Y;
        }

        private void MainCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            finalX = e.GetTouchPoint(null).Position.X;
            finalY = e.GetTouchPoint(null).Position.Y;

            double deltaX = finalX - currentX;


        }
    }
}
