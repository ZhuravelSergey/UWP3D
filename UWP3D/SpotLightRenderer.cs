using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;

namespace UWP3D
{
    public class SpotLightRenderer : LightRenderer
    {
        private SpotLight _spotLight;
        private float _intensivity;
        private Color _color;
        
        public float Intensivity
        {
            get
            {
                return _intensivity;
            }
            set
            {
                if (_spotLight != null)
                {
                    _intensivity = value;

                    _spotLight.InnerConeIntensity = _intensivity;
                    _spotLight.OuterConeIntensity = _intensivity / 2;
                }
            }
        }
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if(_spotLight != null)
                {
                    _color = value;

                    _spotLight.InnerConeColor = _color;
                    _spotLight.OuterConeColor = _color;
                }
            }
        }

        public override void Initialize(Compositor compositor)
        {
            _spotLight = compositor.CreateSpotLight();
            LightSource = _spotLight;
        }
    }
}
