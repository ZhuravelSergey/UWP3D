using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace UWP3D
{
    public class CustomRenderer : Renderer
    {
        public CustomRenderer()
        { }

        public CustomRenderer(Visual visual)
        {
            SetVisual(visual);
        }

        public void SetVisual(Visual visual)
        {
            Graphics = visual;
        }

        public override void Initialize(Compositor compositor)
        { }
    }
}
