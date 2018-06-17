using Cinema.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Cinema
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            int rows = 25;
            int columns = 15;

            Seat[,] seats = new Seat[rows, columns];

            for(int r = 0; r < rows; r++)
            {
                for(int c = 0; c < columns; c++)
                {
                    seats[r, c] = new Seat()
                    {
                         Column = c,
                         Row = r,
                    };
                }
            }

            MyCinema.Seats = seats;
        }
    }
}
