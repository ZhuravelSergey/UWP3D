using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace UWP3D
{
    public class Animation : BehaviourComponent
    {
        private string _property;

        public CompositionAnimation AnimationSource { get; set; }

        public Animation(string targetProperty)
        {
            _property = targetProperty;
        }

        public void Play(string property)
        {
            AnimationSource.StartAnimation(property, AnimationSource);
        }

        public void Stop()
        {
            AnimationSource.StopAnimation(_property);
        }
    }
}
