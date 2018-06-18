using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace UWP3D
{
    public sealed class SceneObject : Behaviour
    {
        private List<SceneObject> _removeChildren;
        private List<SceneObject> _addChildren;
        private List<BehaviourComponent> _removeBehaviour;
        private List<BehaviourComponent> _addBehaviour;
        private SceneObject _parent;
        private List<BehaviourComponent> _behaviours;
        private List<SceneObject> _children;
        private List<Entity> _entities;

        internal bool IsStarted;

        public Scene CurrentScene { get; internal set; }
        public override bool IsActive
        {
            get => base.IsActive;
            set => base.IsActive = value;
        }
        public SceneObject Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                var prev = _parent;

                _parent = value;

                if(prev != null && prev != value)
                {
                    prev.RemoveChild(this);
                    value.AddChild(this);                    
                }
            }
        }
        public Transform Transform { get; private set; }
        public Renderer Renderer { get; set; }

        public IReadOnlyList<BehaviourComponent> Behaviours => _behaviours;
        public IReadOnlyList<SceneObject> Children => _children;
        public List<Entity> Entities
        {
            get => _entities;
            set => _entities = value;
        }

        public SceneObject()
        {
            IsStarted = false;
            _removeChildren = new List<SceneObject>();
            _addChildren = new List<SceneObject>();
            _removeBehaviour = new List<BehaviourComponent>();
            _addBehaviour = new List<BehaviourComponent>();
            _behaviours = new List<BehaviourComponent>();
            _children = new List<SceneObject>();
            _entities = new List<Entity>();
            IsActive = true;

            Transform = new Transform();
        }

        public void AddBehaviour<T>(T behaviour) where T : BehaviourComponent
        {
            _addBehaviour.Add(behaviour);
            behaviour.Owner = this;

            ProcessRequests(IsActive);
        }

        public void RemoveBehaviour(BehaviourComponent behaviour)
        {
            _removeBehaviour.Add(behaviour);
            behaviour.Owner = null;

            ProcessRequests(IsActive);
        }

        public void AddEntity<T>(T entity) where T : Entity
        {
            Entities.Add(entity);
        }

        public void RemoveEntity<T>(T entity) where T : Entity
        {
            Entities.Remove(entity);
        }

        public T GetEntity<T>() where T : Entity
        {
            foreach(var e in Entities)
            {
                if(e is T)
                {
                    return e as T;
                }
            }

            return null;
        }

        public T GetBehaviour<T>() where T : BehaviourComponent
        {
            foreach(var b in Behaviours)
            {
                if(b is T)
                {
                    if(!_removeBehaviour.Contains(b))
                    {
                        return b as T;
                    }
                }
            }
            foreach (var b in _addBehaviour)
            {
                if (b is T)
                {
                    if (!_removeBehaviour.Contains(b))
                    {
                        return b as T;
                    }
                }
            }

            return null;
        }

        internal void RemoveChild(SceneObject child)
        {
            _removeChildren.Add(child);

            ProcessRequests(IsActive);
        }

        internal void AddChild(SceneObject child)
        {
            _addChildren.Add(child);

            ProcessRequests(IsActive);
        }

        public override void Start()
        {
            base.Start();

            ProcessRequests(false);

            if (!IsActive)
            {
                return;
            }

            Behaviours.ExecuteByOrder((e) =>
            {
                e.Start();
            });
            foreach(var c in _children)
            {
                c.Start();
            }

            IsStarted = true;

            ProcessRequests(false);
        }

        public override void Update()
        {
            base.Update();

            ProcessRequests(false);

            if (!IsActive)
            {
                return;
            }

            if(!IsStarted)
            {
                Start();
            }

            Behaviours.ExecuteByOrder((e) =>
            {
                e.Update();
            });
            foreach (var c in _children)
            {
                c.Update();
            }

            ProcessRequests(false);
        }

        internal Visual GetGraphics()
        {
            if(IsActive)
            {
                if(Renderer?.IsActive ?? false)
                {
                    return Renderer.Graphics;
                }
            }

            return null;
        }

        private void ProcessRequests(bool isListsImmutable)
        {
            if(isListsImmutable)
            {
                return;
            }

            foreach(var b in _addBehaviour)
            {
                _behaviours.Add(b);
            }
            foreach(var c in _addChildren)
            {
                _children.Add(c);
            }

            foreach (var b in _removeBehaviour)
            {
                _behaviours.Remove(b);
            }
            foreach(var c in _removeChildren)
            {
                _children.Remove(c);
            }

            _addBehaviour.Clear();
            _addChildren.Clear();
            _removeBehaviour.Clear();
            _removeChildren.Clear();
        }
    }
}
