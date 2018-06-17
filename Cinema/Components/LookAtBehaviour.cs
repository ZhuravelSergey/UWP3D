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
    public class LookAtBehaviour : BehaviourComponent
    {
        public Vector3 Target { get; set; }

        public override void Update()
        {
            base.Update();

            Vector3 position = this.Owner.Transform.Position;

            var a = Math.Abs(position.Z - Target.Z);
            var b = Math.Abs(position.X - Target.X);
            var c = Vector3.Distance(position, Target);

            var cos = a / c;

            var ac = Math.Acos(cos);

            if (position.X > Target.X)
                ac *= -1;

            this.Owner.Transform.Rotation = Vector3.Lerp(this.Owner.Transform.Rotation, new Vector3(0, (float)ac, 0), 0.05f);

            Debug.WriteLine(ac);
        }
    }
}
