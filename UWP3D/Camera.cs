using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Media3D;

namespace UWP3D
{
    public class Camera : CameraBase
    {
        private bool _projectionChanged;

        public Camera(FrameworkElement target)
        {
            Target = target;
        }

        public override void Update()
        {
            base.Update();

            if(_projectionChanged)
            {
                _projectionChanged = false;

                ApplyProjection();
            }
        }

        public override void Start()
        {
            ApplyProjection();

            this.Owner.Transform.PropertyChanged += Transform_PropertyChanged;
        }

        private void Transform_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _projectionChanged = true;
        }

        public override void UpdateView()
        {
            ApplyProjection();
        }

        private void ApplyProjection()
        {
            double fovY = Math.PI / 2.0;
            double translationZ = -Target.ActualHeight / Math.Tan(fovY / 2.0);

            Matrix3D centerImageAtOrigin = TranslationTransform(
             -Target.ActualWidth / 2.0,
             -Target.ActualHeight / 2.0, 0);
            Matrix3D rotateAboutY = RotateYTransform(this.Owner.Transform.Rotation.Y);

            Matrix3D perspective = PerspectiveTransformFovRH(fovY,
                    Target.ActualWidth / Target.ActualHeight,
                    1.0,
                    1000.0);

            Matrix3D viewport = ViewportTransform(Target.ActualWidth, Target.ActualHeight);

            Matrix3D m = centerImageAtOrigin * TranslationTransform(
                -this.Owner.Transform.Position.X + Target.ActualWidth / 2.0, 
                -this.Owner.Transform.Position.Y + Target.ActualHeight / 2.0, 
                -this.Owner.Transform.Position.Z);
            m = m * CreateScaleTransform(1, 1, -1);
            m = m * rotateAboutY;
            m = m * perspective;
            m = m * viewport;

            Matrix3DProjection m3dProjection = new Matrix3DProjection();
            m3dProjection.ProjectionMatrix = m;

            Target.Projection = m3dProjection;
        }

        private Matrix3D TranslationTransform(double tx, double ty, double tz)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = 1.0; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = 1.0; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = tx; m.OffsetY = ty; m.OffsetZ = tz; m.M44 = 1.0;

            return m;
        }

        private Matrix3D CreateScaleTransform(double sx, double sy, double sz)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = sx; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = sy; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = sz; m.M34 = 0.0;
            m.OffsetX = 0.0; m.OffsetY = 0.0; m.OffsetZ = 0.0; m.M44 = 1.0;

            return m;
        }

        private Matrix3D RotateYTransform(double theta)
        {
            double sin = Math.Sin(theta);
            double cos = Math.Cos(theta);

            Matrix3D m = new Matrix3D();

            m.M11 = cos; m.M12 = 0.0; m.M13 = -sin; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = 1.0; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = sin; m.M32 = 0.0; m.M33 = cos; m.M34 = 0.0;
            m.OffsetX = 0.0; m.OffsetY = 0.0; m.OffsetZ = 0.0; m.M44 = 1.0;

            return m;
        }

        private Matrix3D RotateZTransform(double theta)
        {
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            Matrix3D m = new Matrix3D();
            m.M11 = cos; m.M12 = sin; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = -sin; m.M22 = cos; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = 0.0; m.OffsetY = 0.0; m.OffsetZ = 0.0; m.M44 = 1.0;
            return m;
        }

        private Matrix3D PerspectiveTransformFovRH(double fieldOfViewY, double aspectRatio, double zNearPlane, double zFarPlane)
        {
            double height = 1.0 / Math.Tan(fieldOfViewY / 2.0);
            double width = height / aspectRatio;
            double d = zNearPlane - zFarPlane;

            Matrix3D m = new Matrix3D();
            m.M11 = width; m.M12 = 0; m.M13 = 0; m.M14 = 0;
            m.M21 = 0; m.M22 = height; m.M23 = 0; m.M24 = 0;
            m.M31 = 0; m.M32 = 0; m.M33 = zFarPlane / d; m.M34 = -1;
            m.OffsetX = 0; m.OffsetY = 0; m.OffsetZ = zNearPlane * zFarPlane / d; m.M44 = 0;

            return m;
        }

        private Matrix3D ViewportTransform(double width, double height)
        {
            Matrix3D m = new Matrix3D();

            m.M11 = width / 2.0; m.M12 = 0.0; m.M13 = 0.0; m.M14 = 0.0;
            m.M21 = 0.0; m.M22 = -height / 2.0; m.M23 = 0.0; m.M24 = 0.0;
            m.M31 = 0.0; m.M32 = 0.0; m.M33 = 1.0; m.M34 = 0.0;
            m.OffsetX = width / 2.0; m.OffsetY = height / 2.0; m.OffsetZ = 0.0; m.M44 = 1.0;

            return m;
        }
    }
}
