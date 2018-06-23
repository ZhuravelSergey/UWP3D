using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UWP3D.Services;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace UWP3D
{
    public sealed class Scene
    {
        private class SceneObjectContainer
        {
            public SceneObject SceneObject;
            public bool IsGraphicsExist;

            public SceneObjectContainer(SceneObject sceneObject)
            {
                SceneObject = sceneObject;
            }
        }

        private Visual _hostVisual;
        private ContainerVisual _rootContainerVisual;
        private List<SceneObjectContainer> _sceneObjects;
        private List<LightRenderer> _lightCache;

        public Compositor CurrentCompositor { get; private set; }
        public IReadOnlyList<SceneObject> SceneObjects => _sceneObjects.Select(i => i.SceneObject).ToArray();
        public SceneObject Camera { get; set; }
        public ServiceLocator ServiceLocator { get; private set; }

        public Scene(FrameworkElement target)
        {
            ServiceLocator = new ServiceLocator();

            target.UseLayoutRounding = false;

            _sceneObjects = new List<SceneObjectContainer>();
            _lightCache = new List<LightRenderer>();

            Camera = new SceneObject();
            var cameraBehaviour = new Camera(target);
            Camera.AddBehaviour(cameraBehaviour);

            _hostVisual = ElementCompositionPreview.GetElementVisual(target);
            CurrentCompositor = _hostVisual.Compositor;

            _rootContainerVisual = CurrentCompositor.CreateContainerVisual();

            ElementCompositionPreview.SetElementChildVisual(target, _rootContainerVisual);

            Time.LastUpdate = DateTime.UtcNow;
            Time.Delta = 0;
        }

        public void AddObject(SceneObject sceneObject)
        {
            sceneObject.CurrentScene = this;

            var container = new SceneObjectContainer(sceneObject);

            _sceneObjects.Add(container);

            var graphics = sceneObject.GetGraphics();

            if(graphics != null)
            {
                container.IsGraphicsExist = true;

                _rootContainerVisual.Children.InsertAtTop(graphics);
            }
        }

        public void Update()
        {
            UpdateObjects();
            UpdateLight();

            Time.Delta = (DateTime.UtcNow.TimeOfDay.TotalMilliseconds - Time.LastUpdate.TimeOfDay.TotalMilliseconds) / 1000d;
            Time.LastUpdate = DateTime.UtcNow;
        }

        public void UpdateAsync()
        {
            UpdateObjectsAsync();
            UpdateLight();

            Time.Delta = (DateTime.UtcNow.TimeOfDay.TotalMilliseconds - Time.LastUpdate.TimeOfDay.TotalMilliseconds) / 1000d;
            Time.LastUpdate = DateTime.UtcNow;
        }

        private void UpdateLight()
        {

        }

        private void UpdateObjects()
        {
            foreach(var obj in _sceneObjects)
            {
                obj.SceneObject.Update();

                if (obj.SceneObject.Transform.Changed)
                {
                    var gr = obj.SceneObject.GetGraphics();

                    if (obj.SceneObject.Transform.Changed)
                    {
                        if (gr != null)
                        {
                            gr.Offset = obj.SceneObject.Transform.Position;
                            gr.Scale = obj.SceneObject.Transform.Scale;
                            gr.Orientation = Extensions.CreateQuaternion(obj.SceneObject.Transform.Rotation.X,
                                obj.SceneObject.Transform.Rotation.Y, obj.SceneObject.Transform.Rotation.Z);
                        }
                    }

                    obj.SceneObject.Transform.Changed = false;
                }
            }

            Camera.Update();
        }

        private void UpdateObjectsAsync()
        {
            Parallel.ForEach(_sceneObjects, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (obj) =>
                {
                    obj.SceneObject.Update();
                });

            foreach (var obj in _sceneObjects)
            {
                if (obj.SceneObject.Transform.Changed)
                {
                    var gr = obj.SceneObject.GetGraphics();

                    if (obj.SceneObject.Transform.Changed)
                    {
                        if (gr != null)
                        {
                            gr.Offset = obj.SceneObject.Transform.Position;
                            gr.Scale = obj.SceneObject.Transform.Scale;
                            gr.Orientation = Extensions.CreateQuaternion(obj.SceneObject.Transform.Rotation.X,
                                obj.SceneObject.Transform.Rotation.Y, obj.SceneObject.Transform.Rotation.Z);
                        }
                    }

                    obj.SceneObject.Transform.Changed = false;
                }
            }

            Camera.Update();
        }
    }
}
