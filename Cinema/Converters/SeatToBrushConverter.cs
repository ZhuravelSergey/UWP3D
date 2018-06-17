using Cinema.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Cinema.Converters
{
    internal class SeatToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var seat = value as Seat;

            SeatBrushes brushes = parameter as SeatBrushes;

            if (seat.IsBooked)
            {
                return brushes.IsBooked;
            }
            if (seat.IsSelected)
            {
                return brushes.IsSelected;
            }

            return brushes.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
