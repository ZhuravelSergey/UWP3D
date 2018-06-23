using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP3D;

namespace Cinema.Components
{
    public class FPSBehaviour : BehaviourComponent
    {
        private DateTime prev;
        private int iterationsCount;

        public override void Start()
        {
            prev = DateTime.Now;
        }

        public override void Update()
        {
            base.Update();

            iterationsCount++;

            var timeDiff = DateTime.Now - prev;

            if (timeDiff.TotalSeconds >= 10)
            {
                var fps = (iterationsCount / (timeDiff.TotalMilliseconds / 10)) * 100;

                Debug.WriteLine($"FPS: {(int)fps}");

                prev = DateTime.Now;
                iterationsCount = 0;
            }
        }
    }
}
