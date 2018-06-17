using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP3D
{
    public abstract class Behaviour
    {
        public virtual bool IsActive { get; set; }

        public Behaviour()
        {
            IsActive = true;
        }

        public virtual void Start()
        { }

        public virtual void Update()
        { }
    }
}
