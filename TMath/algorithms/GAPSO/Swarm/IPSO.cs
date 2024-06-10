using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmath.algorithms.pso
{
    public interface IPSO
    {
        ISwarm Swarm { get; set; }


        void Start();
        void Stop();
        void Initialization();
    }
}
