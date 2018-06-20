using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.UI.Composition;
using System.Numerics;

namespace UWP3D
{
    public static class Extensions
    {
        public static (SceneObject sceneObject, TRenderer renderer) CreateSceneObjectWithRenderer<TRenderer>(Compositor compositor) 
            where TRenderer : Renderer
        {
            var renderer = Activator.CreateInstance(typeof(TRenderer)) as TRenderer;
            renderer.Initialize(compositor);

            var sceneObject = new SceneObject();
            sceneObject.Renderer = renderer;

            return (sceneObject, renderer);
        }

        public static Quaternion CreateQuaternion(float x, float y, float z)
        {
            float rollOver2 = z * 0.5f;
            float sinRollOver2 = (float)Math.Sin((double)rollOver2);
            float cosRollOver2 = (float)Math.Cos((double)rollOver2);
            float pitchOver2 = y * 0.5f;
            float sinPitchOver2 = (float)Math.Sin((double)pitchOver2);
            float cosPitchOver2 = (float)Math.Cos((double)pitchOver2);
            float yawOver2 = x * 0.5f;
            float sinYawOver2 = (float)Math.Sin((double)yawOver2);
            float cosYawOver2 = (float)Math.Cos((double)yawOver2);
            Quaternion result;
            result.X = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.Y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
            result.Z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.W = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            return result;
        }

        public static Quaternion CreateFromVector(Vector3 vector)
        {
            return CreateQuaternion(vector.X, vector.Y, vector.Z);
        }

        public static float EulerToRadian(float euler)
        {
            return euler * (float)Math.PI / 180.0f;
        }

        public static float RadianToEuler(float radian)
        {
            return radian / ((float)Math.PI / 180.0f);
        }
    }
}
