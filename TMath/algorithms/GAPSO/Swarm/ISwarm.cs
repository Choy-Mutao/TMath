using System.Collections.Generic;

namespace tmath.algorithms.pso
{
    /// <summary>
    /// The set of particles
    /// </summary>
    public interface ISwarm
    {
        void Initialization();
        void Evaluate();
        void Move();
    }
}
