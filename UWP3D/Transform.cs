using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D
{
    public class Transform : Entity
    {
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;

        internal bool Changed { get; set; }

        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                RaisePropertyChanged();

                Changed = true;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;

                RaisePropertyChanged();

                Changed = true;
            }
        }
        public Vector3 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;

                RaisePropertyChanged();

                Changed = true;
            }
        }

        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

            Changed = true;
        }
    }
}
