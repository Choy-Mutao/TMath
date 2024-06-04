using System.Collections.Generic;

namespace tmath.algorithms.pso
{
    /// <summary>
    /// A particle is made of 
    /// 1. a position inside at this position
    /// 2. the fitness value at this position
    /// 3. a velocity(in fact a displacement), which will be used to compute the next position
    /// 4. a memory, that contains the best position(called the previous best) found so far by the particle
    /// 5. the fitness value of this previous best
    /// 
    /// NOTE: in this project, all geometry params use 2D message;
    /// </summary>
    public class Particle
    {
        private TPoint2D Position; // a position inside at this position;
        private TVector2D Velocity; // a velocity which will be used to compute the next position;

        private object Data;
        private object Memory; // contains the best position (called the previous best) found so far by the particle;

        private float Fitness; // the fitness value at this position
        private float Previous_Fitness; // the fitness value of this previous best

        public List<Particle> GetNeighbours(double radius)
        {
        }


        /// <summary>
        /// The velocity update equations
        /// </summary>

        public void UpdateSpeed()
        {

        }

        /// <summary>
        /// Move by applying the new velocity to the current position
        /// </summary>
        public void Move()
        {

        }

        public double Evaluate()
        {

        }
    }

    /// <summary>
    /// The set of particles
    /// </summary>
    public interface ISwarm
    {
        List<Particle> GetNeighbours(in Particle particle, double radius);
        void Initialization();
    }
}
