using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D
{
    public abstract class BehaviourComponent : Behaviour
    {
        public SceneObject Owner { get; internal set; }
    }
}
