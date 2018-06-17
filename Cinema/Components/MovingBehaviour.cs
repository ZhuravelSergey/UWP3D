using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class MovingBehaviour : BehaviourComponent
    {
        public override void Update()
        {
            base.Update();

            var p = this.Owner.Transform.Position;

            this.Owner.Transform.Position = new System.Numerics.Vector3(p.X - 3, p.Y, p.Z);
        }        
    }
}
