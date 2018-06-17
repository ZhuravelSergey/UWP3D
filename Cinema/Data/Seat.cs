using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Data
{
    public class Seat : BindableBase
    {
        private int _row;
        private int _column;
        private bool _isSelected;
        private bool _isBooked;

        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;

                RaisePropertyChanged();
            }
        }
        public int Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;

                RaisePropertyChanged();
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                RaisePropertyChanged();
            }
        }
        public bool IsBooked
        {
            get
            {
                return _isBooked;
            }
            set
            {
                _isBooked = value;

                RaisePropertyChanged();

                IsSelected = false;
            }
        }
    }
}
