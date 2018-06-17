using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;

namespace UWP3D
{
    public abstract class Renderer : BehaviourComponent
    {
        public Visual Graphics { get; protected set; }

        public abstract void Initialize(Compositor compositor);
    }
}
