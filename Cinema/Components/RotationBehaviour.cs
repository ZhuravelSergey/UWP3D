using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class RotationBehaviour : BehaviourComponent
    {
        public override void Update()
        {
            var x = this.Owner.Transform.Rotation.X + (float)Time.Delta / 2f;

            this.Owner.Transform.Rotation = new Vector3(x, 0, 0);

            //Debug.WriteLine(x);
        }
    }
}
