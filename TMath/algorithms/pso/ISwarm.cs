using System.Collections.Generic;

namespace tmath.algorithms.pso
{
    /// <summary>
    /// The set of particles
    /// </summary>
    public interface ISwarm
    {
        List<Particle> GetNeighbours(in Particle particle, double radius);
        void Initialization();
        void Evaluate();
        void Move();
    }
}
