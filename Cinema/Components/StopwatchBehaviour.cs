using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class StopwatchBehaviour : BehaviourComponent
    {
        public override void Update()
        {
            base.Update();

            Debug.WriteLine($"Frame delay: {Time.Delta}");
        }
    }
}
