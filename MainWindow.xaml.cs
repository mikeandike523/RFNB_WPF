using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
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

        //Application Parameters
        private readonly double _swipeDeltaX = 50.0;

        //Notebook Function

        Notebook notebook = new Notebook("Untitled");


        //UI -- Window and Rendering

        public MainWindow()
        {

            InitializeComponent();

            // --- Courtesy of Microsoft Documentation
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.inkcanvaseditingmode?view=windowsdesktop-6.0#system-windows-controls-inkcanvaseditingmode-ink
            MainInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            // ---

            Render();
        }

        private void Render() {

            StrokeCollection? collection = notebook.LoadNotebookPageStrokes(null);
            if (collection != null) { 
                MainInkCanvas.Strokes = collection;
            }
            PageNumber.Text = (notebook.GetCurrentPageNumber()+1).ToString();
        
        }

        //UI -- User Input

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
            if (deltaX < -_swipeDeltaX)
            {
                notebook.SwitchToNextPage();
                Render();
            }
            else if (deltaX > -_swipeDeltaX)
            {
                notebook.SwitchToPreviousPage();
                Render();
            }
            else { 
            
            }

        }

        private void MainInkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            notebook.WriteStrokesToNotebook(null, MainInkCanvas.Strokes);
        }

        private void MainInkCanvas_StrokeErased(object sender, RoutedEventArgs e)
        {
            notebook.WriteStrokesToNotebook(null, MainInkCanvas.Strokes);
        }
    }
}
