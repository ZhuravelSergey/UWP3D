using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Composition;

namespace UWP3D
{
    public class SpriteRenderer : Renderer
    {
        private SpriteVisual _spriteVisual;

        public CompositionBrush Brush
        {
            get => _spriteVisual.Brush;
            set => _spriteVisual.Brush = value;
        }
        public Vector2 Size
        {
            get => _spriteVisual.Size;
            set => _spriteVisual.Size = value;
        }

        public override void Initialize(Compositor compositor)
        {
            _spriteVisual = compositor.CreateSpriteVisual();
            Graphics = _spriteVisual;
        }
        
        public void EnableShadow()
        {
            DropShadow shadow = Graphics.Compositor.CreateDropShadow();
            shadow.BlurRadius = 15;
            shadow.Offset = new Vector3(5, 5, -1);
            shadow.Color = Colors.Black;
            
            _spriteVisual.Shadow = shadow;
        }
        
        public void DisableShadow()
        {
            _spriteVisual.Shadow = null;
        }
    }
}
