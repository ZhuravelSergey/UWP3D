using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class SmoothMovementBehaviour : BehaviourComponent
    {
        private Vector3? _target = null;

        public void MoveTo(Vector3 target)
        {
            _target = target;
        }

        public override void Update()
        {
            base.Update();

            if(_target != null)
            {
                var target = _target.Value;

                var position = this.Owner.Transform.Position;
                var current_y = position.Y;

                if (Vector3.Distance(target, position) < 1.0f)
                {
                    _target = null;
                }
                else
                {
                    this.Owner.Transform.Position = Vector3.Lerp(position, target, 0.05f);
                }
            }
        }
    }
}
