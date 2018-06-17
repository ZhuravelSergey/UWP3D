using Cinema.Converters;
using Cinema.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Cinema
{
    public sealed partial class BookingPlatform : UserControl
    {
        private static SeatToBrushConverter _seatToBrushConverter;
        private static SeatBrushes _seatToBrushConverterParameter;

        public static DependencyProperty SeatsProperty { get; private set; }

        public Seat[,] Seats
        {
            get => GetValue(SeatsProperty) as Seat[,];
            set => SetValue(SeatsProperty, value);
        }

        static BookingPlatform()
        {
            _seatToBrushConverter = new SeatToBrushConverter();
            _seatToBrushConverterParameter = new SeatBrushes()
            {
                Default = new SolidColorBrush(Colors.DimGray),
                IsSelected = new SolidColorBrush(Colors.Green),
                IsBooked = new SolidColorBrush(Colors.Red)
            };

            SeatsProperty = DependencyProperty.Register("Seats", typeof(Seat[,]),
                typeof(BookingPlatform), new PropertyMetadata(null, SeatsChanged));
        }

        public BookingPlatform()
        {
            this.InitializeComponent();
        }

        private static void SeatsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bp = d as BookingPlatform;

            var container = bp.Container;

            var seats = bp.Seats;

            var rows = seats.GetLength(0);
            var columns = seats.GetLength(1);

            if (rows * columns > 0)
            {
                container.Rows = rows;
                container.Columns = columns;

                for (int r = rows - 1; r > 0; r--)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        var s = seats[r, c];

                        var item = CreateSeatItem(s);

                        container.Children.Add(item);

                        item.Margin = new Thickness(5);
                        item.HorizontalAlignment = HorizontalAlignment.Stretch;
                        item.VerticalAlignment = VerticalAlignment.Stretch;
                    }
                }
            }
            else
            {
                container.Rows = 0;
                container.Columns = 0;

                container.Children.Clear();
            }
        }

        private static FrameworkElement CreateSeatItem(Seat dataContext)
        {
            Button button = new Button();
            button.DataContext = dataContext;
            button.SetBinding(Button.BackgroundProperty, new Binding()
            {
                Converter = _seatToBrushConverter,
                Path = new PropertyPath(""),
                ConverterParameter = _seatToBrushConverterParameter
            });

            button.Click += delegate
            {
                if (!dataContext.IsBooked)
                {
                    dataContext.IsSelected = !dataContext.IsSelected;
                }
            };

            dataContext.PropertyChanged += delegate
            {
                button.DataContext = null;
                button.DataContext = dataContext;
            };

            Grid grid = new Grid();

            TextBlock tb = new TextBlock();
            //tb.Text = $"{dataContext.Row}.{dataContext.Column}";

            grid.Children.Add(tb);

            button.Content = grid;

            return button;
        }
    }
}
