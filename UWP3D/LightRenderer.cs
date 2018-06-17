using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace UWP3D
{
    public abstract class LightRenderer : Renderer
    {
        private List<WeakReference<SceneObject>> _target;

        internal bool Changed { get; set; }

        public CompositionLight LightSource { get; protected set; }
        public IReadOnlyCollection<SceneObject> Target
        {
            get
            {
                ClearEmptyTargets();

                return GetTargets() as IReadOnlyCollection<SceneObject>;
            }
        }

        public LightRenderer()
        {
            _target = new List<WeakReference<SceneObject>>();

            Changed = true;
        }

        public void AddTarget(SceneObject so)
        {
            _target.Add(new WeakReference<SceneObject>(so));

            Changed = true;
        }

        public void RemoveTarget(SceneObject so)
        {
            foreach(var t in _target)
            {
                if(t.TryGetTarget(out var l))
                {
                    if(l == so)
                    {
                        _target.Remove(t);

                        Changed = true;

                        break;
                    }
                }
            }
        }

        protected IReadOnlyCollection<SceneObject> GetTargets()
        {
            if (_target.Count > Environment.ProcessorCount * 50)
            {
                ConcurrentBag<SceneObject> items = new ConcurrentBag<SceneObject>();

                Parallel.ForEach(_target, (t) =>
                {
                    if (!t.TryGetTarget(out var i))
                    {
                        items.Add(i);
                    }
                });

                return items;
            }
            else
            {
                List<SceneObject> items = new List<SceneObject>();

                foreach (var t in _target)
                {
                    if (!t.TryGetTarget(out var i))
                    {
                        items.Add(i);
                    }
                }

                return items;
            }
        }

        protected void ClearEmptyTargets()
        {
            if(_target.Count > Environment.ProcessorCount * 50)
            {
                ConcurrentBag<WeakReference<SceneObject>> remove = new ConcurrentBag<WeakReference<SceneObject>>();

                Parallel.ForEach(_target, (t) =>
                {
                    if (!t.TryGetTarget(out _))
                    {
                        remove.Add(t);
                    }
                });

                foreach (var r in remove)
                {
                    _target.Remove(r);
                }
            }
            else
            {
                List<WeakReference<SceneObject>> remove = new List<WeakReference<SceneObject>>();

                foreach(var t in _target)
                {
                    if(!t.TryGetTarget(out _))
                    {
                        remove.Add(t);
                    }
                }

                foreach(var r in remove)
                {
                    _target.Remove(r);
                }
            }
        }
    }
}
