using Cinema.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class SeatBehaviour : BehaviourComponent
    {
        private Entity<Seat> _seat;
        private SmoothMovementBehaviour _smoothMovementBehaviour;
        private float _startY;

        public override void Start()
        {
            base.Start();

            _startY = this.Owner.Transform.Position.Y;

            _smoothMovementBehaviour = this.Owner.GetBehaviour<SmoothMovementBehaviour>();

            _seat = this.Owner.GetEntity<Entity<Seat>>();

            _seat.PropertyChanged += Entity_PropertyChanged;
        }

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _smoothMovementBehaviour.MoveTo(new System.Numerics.Vector3(this.Owner.Transform.Position.X,
                _seat.Value.IsSelected ? 75 : _startY, this.Owner.Transform.Position.Z));
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
