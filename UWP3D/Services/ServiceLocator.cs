using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D.Services
{
    public class ServiceLocator
    {
        private ConcurrentDictionary<int, object> _services;
        private object _lock;

        public ServiceLocator()
        {
            _services = new ConcurrentDictionary<int, object>();
            _lock = new object();
        }

        public void Add<T>(T value) where T : class
        {
            lock(_lock)
            {
                var hash = typeof(T).GetHashCode();

                if(value == null)
                {
                    throw new NullReferenceException();
                }

                if(_services.ContainsKey(hash))
                {
                    throw new ArgumentException("Service with same type is already exist!");
                }

                _services.TryAdd(hash, value);
            }
        }

        public T Get<T>() where T : class
        {
            var hash = typeof(T).GetHashCode();

            if(!_services.ContainsKey(hash))
            {
                throw new KeyNotFoundException();
            }
            else
            {
                return _services[hash] as T;
            }
        }
    }
}
