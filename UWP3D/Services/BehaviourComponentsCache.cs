using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D.Services
{
    internal class BehaviourComponentsCache
    {
        private List<BehaviourComponent> _behaviours;
        private bool _sort;

        public BehaviourComponentsCache()
        {
            _behaviours = new List<BehaviourComponent>();
        }

        public void Add<T>(T behaviour) where T : BehaviourComponent
        {
            _behaviours.Add(behaviour);
            _sort = true;
        }

        public void Remove<T>(T behaviour) where T : BehaviourComponent
        {
            _behaviours.Remove(behaviour);
        }

        public IReadOnlyList<BehaviourComponent> GetBehaiours()
        {
            return _behaviours as IReadOnlyList<BehaviourComponent>;
        }

        internal List<BehaviourComponent> GetBehavioursList()
        {
            return _behaviours;
        }

        public void Start()
        {
            if(_sort)
            {
                Sort();
            }

            foreach(var b in _behaviours)
            {
                b.Start();
            }
        }

        public void Update()
        {
            if(_sort)
            {
                Sort();
            }

            foreach(var b in _behaviours)
            {
                b.Update();
            }
        }

        private void Sort()
        {
            List<(int, BehaviourComponent)> items = new List<(int, BehaviourComponent)>();

            Dictionary<int, int> cache = new Dictionary<int, int>();

            foreach (var e in _behaviours)
            {
                var type = e.GetType();

                int hc = type.GetHashCode();

                if (!cache.ContainsKey(hc))
                {
                    var eo = type.GetTypeInfo().GetCustomAttribute<ExecutionOrder>();

                    if (eo == null)
                    {
                        cache.Add(hc, int.MaxValue);
                    }
                    else
                    {
                        cache.Add(hc, eo.Order);
                    }
                }

                items.Add((cache[hc], e));
            }

            var sorted = items.OrderBy(i => i.Item1);

            _behaviours.Clear();

            foreach (var e in sorted)
            {
                _behaviours.Add(e.Item2);
            }

            _sort = false;
        }
    }
}
