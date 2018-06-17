using Cinema.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class SeatEntity : Entity<Seat>
    {
        private bool? _isSelected;

        public event SeatSelectionChangedEventHandler SelectionChanged;

        public SeatEntity()
        {
            this.PropertyChanged += SeatEntity_PropertyChanged;
        }

        public SeatEntity(Seat seat)
        {
            Value = seat;

            this.PropertyChanged += SeatEntity_PropertyChanged;

            _isSelected = Value.IsSelected;
        }

        private void SeatEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(_isSelected == null)
            {
                _isSelected = Value.IsSelected;
            }
            else
            {
                if(_isSelected.Value != Value.IsSelected)
                {
                    _isSelected = Value.IsSelected;

                    SelectionChanged?.Invoke(Value);
                }
            }
        }
    }
}
