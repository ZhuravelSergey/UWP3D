using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWP3D
{
    public abstract class CameraBase : BehaviourComponent
    {
        public FrameworkElement Target { get; protected set; }

        public abstract void UpdateView();
    }
}
