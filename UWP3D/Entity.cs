using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D
{
    public abstract class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Entity<T> : Entity
    {
        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                var prev = _value;

                _value = value;

                RaisePropertyChanged();

                INotifyPropertyChanged npc = _value as INotifyPropertyChanged;

                if(npc != null)
                {
                    npc.PropertyChanged += Value_PropertyChanged;
                }
                if(prev != null)
                {
                    INotifyPropertyChanged pnpc = prev as INotifyPropertyChanged;

                    if(pnpc != null)
                    {
                        pnpc.PropertyChanged -= Value_PropertyChanged;
                    }
                }
            }
        }

        public Entity()
        {
            Value = default(T);
        }

        public Entity(T value)
        {
            Value = value;
        }

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged();
        }

        public static explicit operator T(Entity<T> value)
        {
            return value.Value;
        }
    }
}
